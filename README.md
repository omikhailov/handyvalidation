# Handy validation library for WinUI and UWP

Nuget Packages:
* [HandyValidation (.Net Standard 2.0 library)](https://www.nuget.org/packages/HandyValidation)
* [HandyValidation.Resources for WinUI / .Net 6.0 and UWP 10.0.16299+](https://www.nuget.org/packages/HandyValidation.Resources)
* [HandyValidation.UI for WinUI / .Net 6.0 and UWP 10.0.16299+](https://www.nuget.org/packages/HandyValidation.UI)
* [HandyValidation.UI.WPF for WPF / .Net 6.0](https://www.nuget.org/packages/HandyValidation.UI.WPF)

HandyValidation allows you to significantly simplify and structure input validation code in your WinUI and UWP applications. Let's start right away with some examples and see how it would work with a typical form.

![image](https://user-images.githubusercontent.com/75426711/205976050-755ed665-52a0-4396-b8af-173e9ffa189c.png)

```csharp
    public class MainViewModel
    {
        public MainViewModel()
        {
            SetupValidation();
        }

        public Property<string> FirstName = new("John")
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithResourceString("FirstNameCannotBeEmpty"),
                Rule.MinLength(2).WithFormattedResourceString("FirstNameCannotBeInitials")),
        };

        public Property<string> LastName = new("Smith")
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please fill the Last Name field"),
                Rule.MinLength(2).WithMessage("Last Name must be at least two characters long")),
        };

        public Property<DateTimeOffset> Dob = new(DateTimeOffset.Now, ValidatorState.Invalid)
        {
            Validator = new RulesValidator<DateTimeOffset>(
                Rule.Range(DateTimeOffset.Now.AddYears(-60), DateTimeOffset.Now.AddYears(-21)).WithMessage("The borrower must be at least 21 and no older than 60"))
        };

        public Property<string> PhoneNumber = new()
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please enter phone number"),
                Rule.NumberOfDigits(8, 11).WithMessage("Please enter either 8-digit local number or 11-digit mobile number"),
                Rule.AllowedSymbols("+()- 0123456789").WithMessage("The phone number you entered contains invalid characters")),
            ValueChanged = info => { info.Property.Metadata = string.Concat(info.NewValue.Where(c => char.IsDigit(c))); }
        };

        public Property<string> Email = new()
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please enter email address"),
                Rule.Email().WithMessage("Email address is incorrect")),
            Delay = TimeSpan.FromSeconds(0.8)
        };

        public Property<string> Password = new()
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please enter the password"),
                Rule.LengthIsInRange(8, 20).WithMessage("Password length must be between eight and twenty characters"),
                Rule.Custom<string>(password =>
                {
                    if (password == null) return null;

                    if (!password.Any(c => char.IsLetter(c)) || !password.Any(c => char.IsDigit(c)) || !password.Any(c => !char.IsLetterOrDigit(c)))
                    {
                        return "Password must contain at least one letter, digit and special character";
                    }

                    return null;
                }))
        };

        public Property<string> ConfirmPassword = new();

        public CustomValidator ConfirmPasswordValidator;

        public CustomValidator ApiAvailabilityValidator = new CustomValidator(async (issues, token) => 
        {
            await Task.Delay(500, token);

            issues.Add("Unfortunately, we cannot accept your application right now because our server is temporarily down. Our experts are already working on fixing this problem. Please try again later.");
        });

        public CompositeValidator FormValidator;

        public ValidationStateWatcher SubmitButtonWatcher;

        public InputSwitch FormSwitch;

        private void SetupValidation()
        {
            var properties = Property.List(FirstName, LastName, Dob, PhoneNumber, Email, Password);

            ConfirmPasswordValidator = new CustomValidator(ValidatePasswordsMatch);

            ConfirmPassword.ValueChangedAsync = async info => { await ConfirmPasswordValidator.Validate(info.CancellationToken); };

            FormValidator = new CompositeValidator(properties, ConfirmPasswordValidator, ApiAvailabilityValidator);
            
            SubmitButtonWatcher = new ValidationStateWatcher(properties, ConfirmPasswordValidator);

            FormSwitch = new InputSwitch(properties, ConfirmPassword, SubmitButtonWatcher);
        }

        private Task ValidatePasswordsMatch(ObservableCollection<object> issues, CancellationToken token)
        {
            if (Password.Value != null && !Password.Value.Equals(ConfirmPassword.Value, StringComparison.Ordinal)) issues.Add("Passwords do not match");

            return Task.CompletedTask;
        }

        public async Task Submit()
        {
            await FormSwitch.OffWhile(FormValidator.Validate());

            if (!FormValidator.HasIssues)
            {
                // Submit
            }
        }
    }
```

```xaml
    <Grid x:Name="Root">
        <Grid.DataContext>
            <vm:MainViewModel x:Name="ViewModel" />
        </Grid.DataContext>

        <Grid.Resources>
            <ResourceDictionary>
                <Style TargetType="TextBlock" x:Key="ValidationMessage">
                    <Setter Property="Foreground" Value="#C42B1C" />
                    <Setter Property="TextWrapping" Value="Wrap" />
                </Style>
                <Style x:Key="InvalidLastName" TargetType="TextBox">
                    <Setter Property="Background" Value="LightPink" />
                </Style>
                <DataTemplate x:Key="CustomValidationPopupItem" x:DataType="x:Object">
                    <Grid>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="Auto" />
                            <ColumnDefinition />
                        </Grid.ColumnDefinitions>
                        <SymbolIcon Symbol="Important" Height="16" VerticalAlignment="Top" Margin="0,6,8,0" />
                        <TextBlock Grid.Column="1" Text="{x:Bind}" TextWrapping="Wrap" VerticalAlignment="Top" Margin="0,0,8,0" />
                    </Grid>
                </DataTemplate>
                <ThemeShadow x:Name="DefaultShadow" />
            </ResourceDictionary>
        </Grid.Resources>

        <ScrollViewer>
            <StackPanel HorizontalAlignment="Center" Width="400" Padding="8,64,8,24">

                <TextBox x:Name="FirstName" Header="First Name" Margin="0,8,0,0"
                         Text="{x:Bind ViewModel.FirstName.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                         IsEnabled="{x:Bind ViewModel.FirstName.IsEnabled, Mode=OneWay}"
                         validation:Border.IsHighlighted="{x:Bind ViewModel.FirstName.Validator.HasIssues, Mode=OneWay}" />
                <TextBlock Text="{x:Bind ViewModel.FirstName.Validator.FirstIssue, Mode=OneWay}"
                           Visibility="{x:Bind ViewModel.FirstName.Validator.HasIssues, Mode=OneWay}" Style="{StaticResource ValidationMessage}" />

                <TextBox Header="Last Name" Margin="0,8,0,0"
                         Text="{x:Bind ViewModel.LastName.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                         IsEnabled="{x:Bind ViewModel.LastName.IsEnabled, Mode=OneWay}"
                         validation:Style.Value="{StaticResource InvalidLastName}" 
                         validation:Style.IsApplied="{x:Bind ViewModel.LastName.Validator.HasIssues, Mode=OneWay}" />

                <DatePicker Header="Date of Birth" HorizontalAlignment="Stretch" Margin="0,8,0,0"
                            Date="{x:Bind ViewModel.Dob.Value, Mode=TwoWay}"
                            IsEnabled="{x:Bind ViewModel.Dob.IsEnabled, Mode=OneWay}"
                            validation:Popup.IsOpen="{x:Bind ViewModel.Dob.Validator.HasIssues, Mode=OneWay}"
                            validation:Popup.ItemsSource="{x:Bind ViewModel.Dob.Validator.Issues}" />

                <TextBox Header="Phone Number" Margin="0,8,0,0"
                         Text="{x:Bind ViewModel.PhoneNumber.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                         IsEnabled="{x:Bind ViewModel.PhoneNumber.IsEnabled, Mode=OneWay}"
                         validation:Popup.IsOpen="{x:Bind ViewModel.PhoneNumber.Validator.HasIssues, Mode=OneWay}"
                         validation:Popup.ItemsSource="{x:Bind ViewModel.PhoneNumber.Validator.Issues}" />

                <TextBox Header="Email" Margin="0,8,0,0"
                         Text="{x:Bind ViewModel.Email.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                         IsEnabled="{x:Bind ViewModel.Email.IsEnabled, Mode=OneWay}"
                         validation:Border.IsHighlighted="{x:Bind ViewModel.Email.Validator.HasIssues, Mode=OneWay}"
                         validation:Popup.IsOpen="{x:Bind ViewModel.Email.Validator.HasIssues, Mode=OneWay}"
                         validation:Popup.ItemsSource="{x:Bind ViewModel.Email.Validator.Issues}" />

                <PasswordBox Header="Password" Margin="0,8,0,0"
                             Password="{x:Bind ViewModel.Password.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                             IsEnabled="{x:Bind ViewModel.Password.IsEnabled, Mode=OneWay}"
                             validation:Popup.IsOpen="{x:Bind ViewModel.Password.Validator.HasIssues, Mode=OneWay}"
                             validation:Popup.ItemsSource="{x:Bind ViewModel.Password.Validator.Issues}"
                             validation:Popup.ItemTemplate="{StaticResource CustomValidationPopupItem}" />

                <PasswordBox Header="Confirm Password" Margin="0,8,0,0"
                             Password="{x:Bind ViewModel.ConfirmPassword.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                             IsEnabled="{x:Bind ViewModel.ConfirmPassword.IsEnabled, Mode=OneWay}"
                             validation:Popup.IsOpen="{x:Bind ViewModel.ConfirmPasswordValidator.HasIssues, Mode=OneWay}"
                             validation:Popup.ItemsSource="{x:Bind ViewModel.ConfirmPasswordValidator.Issues}" />

                <Button Content="Submit" IsEnabled="{x:Bind ViewModel.SubmitButtonWatcher.IsEnabled, Mode=OneWay}" Click="{x:Bind ViewModel.Submit}" HorizontalAlignment="Right" Margin="0,32,0,0" />

            </StackPanel>
        </ScrollViewer>

        <ContentDialog Title="Custom Dialog" CloseButtonText="OK"
                       validation:ContentDialog.IsOpen="{x:Bind ViewModel.FormValidator.HasIssues, Mode=OneWay}">
            <Grid MinWidth="300">
                <ItemsControl ItemsSource="{x:Bind ViewModel.FormValidator.Issues}" Margin="32,16,32,16">
                    <ItemsControl.ItemTemplate>
                        <DataTemplate x:DataType="x:Object">
                            <TextBlock Text="{x:Bind}" TextWrapping="Wrap" />
                        </DataTemplate>
                    </ItemsControl.ItemTemplate>
                </ItemsControl>
            </Grid>
        </ContentDialog>
    </Grid>
```
As you can see, everything looks quite simple and straight forward - each input control is bound to an instance of a special **Property&lt;T&gt;** class in the view model, which has a property of type **IValueValidator&lt;T&gt;**. **RulesValidator&lt;T&gt;** implements this interface and can be initialized with a set of predefined or custom **IValidationRule&lt;T, I&gt;**.

In addition to value validators, there are also validators that are not associated with specific fields. This example uses **CustomValidator** to check for password matches and network availability, and **CompositeValidator** to validate the state of entire form. There is also **ValidationStateWatcher** keeping Submit button disabled until all properties will have valid value.

Predefined validation rules do not return error messages, it is up to you to define them. There are several extension methods allowing you to add static message or use a resource strings from the **.resx** files: **WithMessage()**, **WithFormattedMessage()**, **WithResourceString()**, **WithFormattedResourceString()**. The last two groups of methods are in **HandyValidation.Resources** package.

And finally, you can look at PhoneNumber and Email properties demonstrating some minor features. For Email, there is an input delay, and in the case of PhoneNumber, you can see how to handle moments when the property value changes. In this example, **ValueChanged** delegate takes the phone number entered by user and saves the value without + - and () symbols into **Property.Metadata**, which is a field of type object where you can store whatever you want. In addition to ValueChanged there is also **ValueChanging** and asynchronous **ValueChangingAsync** and **ValueChangedAsync**. It is important to note that for asynchronous versions, Property passes the **CancellationToken** which will become cancelled if user will continue typing and your async code will still be running at the moment when value changes once again.

Now let's move from view model layer to the view where **HandyValidation.UI** provides you with following options:
- **Border** service and its attached properties **IsHighlighted** and **HighlightingBrush**
- **Style** service with properties **IsApplied** and **Value**
- **Popup** service with properties: **IsOpen**, **ItemsSource**, **ItemTemplate**, **Background**, **Foreground** and others

**Border** allows you to change BorderBrush of control to indicate validation error

![image](https://user-images.githubusercontent.com/75426711/205995796-970e2ba6-011c-4ae5-8a00-54eb47445ba7.png)

**Style** is similar but with this service you can change control style. In this example custom style changes BackgroundBrush of the TextBox

![image](https://user-images.githubusercontent.com/75426711/205994886-91c89301-eee0-4c61-b832-c46d712e2abc.png)

And, finally, **Popup** service allows you to associate validation Popup with a control

![image](https://user-images.githubusercontent.com/75426711/207086233-75e7c820-50de-4a3e-8a97-ef82b1836bfb.png)

In addition to these three services, there is also **ContentDialog** which property **IsOpen** makes ContentDialog able to be used with MVVM design and bindings

```xaml
        <ContentDialog Title="Custom Dialog" CloseButtonText="OK"
                       validation:ContentDialog.IsOpen="{x:Bind ViewModel.FormValidator.HasIssues, Mode=OneWay}">
            ...
        </ContentDialog>
```

## Overriding default styles (WinUI & UWP)

To override default styles, add following XAML into App.xaml file of your application and edit default values:

```xaml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

                <ResourceDictionary>
                    <!--<DataTemplate x:Key="ValidationDefaultPopupItemTemplate" />-->
                    <!--<Thickness x:Key="ValidationDefaultPopupBorderThickness">0</Thickness>-->
                    <CornerRadius x:Key="ValidationDefaultPopupCornerRadius">8</CornerRadius>
                    <!--<x:Double x:Key="ValidationDefaultPopupMaxWidth">320</x:Double>-->
                    <!--<x:Double x:Key="ValidationDefaultPopupMinWidth">0</x:Double>-->
                    <!--<x:Double x:Key="ValidationDefaultPopupWidth">320</x:Double>-->
                    <Thickness x:Key="ValidationDefaultPopupPadding">20, 8, 16, 12</Thickness>

                    <ResourceDictionary.ThemeDictionaries>
                        <ResourceDictionary x:Key="Light">
                            <SolidColorBrush x:Key="ValidationDefaultBorderHighlightingBrush" Color="#C4281C" />

                            <!--<SolidColorBrush x:Key="ValidationDefaultPopupBorderBrush" Color="#FDE7E9" />-->
                            <SolidColorBrush x:Key="ValidationDefaultPopupBackgroundBrush" Color="#FDE7E9" />
                            <StaticResource x:Key="ValidationDefaultPopupForegroundBrush" ResourceKey="SystemControlForegroundBaseHighBrush" />
                        </ResourceDictionary>
                        <ResourceDictionary x:Key="Dark">
                            <SolidColorBrush x:Key="ValidationDefaultBorderHighlightingBrush" Color="#C4281C" />

                            <!--<SolidColorBrush x:Key="ValidationDefaultPopupBorderBrush" Color="#442726" />-->
                            <SolidColorBrush x:Key="ValidationDefaultPopupBackgroundBrush" Color="#442726" />
                            <StaticResource x:Key="ValidationDefaultPopupForegroundBrush" ResourceKey="SystemControlForegroundBaseHighBrush" />
                        </ResourceDictionary>
                        <ResourceDictionary x:Key="HighContrast">
                            <SolidColorBrush x:Key="ValidationDefaultBorderHighlightingBrush" Color="#C4281C" />

                            <!--<SolidColorBrush x:Key="ValidationDefaultPopupBorderBrush" Color="{ThemeResource SystemColorWindowColor}" />-->
                            <SolidColorBrush x:Key="ValidationDefaultPopupBackgroundBrush" Color="{ThemeResource SystemColorWindowColor}" />
                            <StaticResource x:Key="ValidationDefaultPopupForegroundBrush" ResourceKey="SystemControlForegroundBaseHighBrush" />
                        </ResourceDictionary>
                    </ResourceDictionary.ThemeDictionaries>
                </ResourceDictionary>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

## Overriding default styles (WPF)

To override default styles, add following XAML into App.xaml file of your application and edit default values:

```xaml
    <!--<DataTemplate x:Key="ValidationDefaultPopupItemTemplate" />-->
    <Thickness x:Key="ValidationDefaultPopupBorderThickness">0</Thickness>
    <CornerRadius x:Key="ValidationDefaultPopupCornerRadius">8</CornerRadius>
    <!--<x:Double x:Key="ValidationDefaultPopupMaxWidth">320</x:Double>-->
    <!--<x:Double x:Key="ValidationDefaultPopupMinWidth">0</x:Double>-->
    <!--<x:Double x:Key="ValidationDefaultPopupWidth">320</x:Double>-->
    <Thickness x:Key="ValidationDefaultPopupPadding">20, 8, 16, 12</Thickness>

    <SolidColorBrush x:Key="ValidationDefaultBorderHighlightingBrush" Color="#C4281C" />
    
    <!--<SolidColorBrush x:Key="ValidationDefaultPopupBorderBrush" Color="#FDE7E9" />-->
    <SolidColorBrush x:Key="ValidationDefaultPopupBackgroundBrush" Color="#FDE7E9" />
    <SolidColorBrush x:Key="ValidationDefaultPopupForegroundBrush" Color="Black" />
```
