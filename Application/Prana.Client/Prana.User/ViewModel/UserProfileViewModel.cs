using Prana.Admin.BLL;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.User.BAL;
using System;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Prana.User.ViewModel
{
    public class UserProfileViewModel : BindableBase
    {

        #region Properties

        /// <summary>
        /// Window User Is Required
        /// </summary>
        bool _isWindowUserReq = CachedDataManager.GetInstance.IsWindowUserRequired();

        private int _userID = int.MinValue;
        public int UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value); }
        }

        private string _firstName = string.Empty;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _login = string.Empty;

        private string _mail = string.Empty;
        public string Mail
        {
            get { return _mail; }
            set { SetProperty(ref _mail, value); }
        }

        private string _addressLine1 = string.Empty;
        public string AddressLine1
        {
            get { return _addressLine1; }
            set { SetProperty(ref _addressLine1, value); }
        }

        private string _addressLine2 = string.Empty;
        public string AddressLine2
        {
            get { return _addressLine2; }
            set { SetProperty(ref _addressLine2, value); }
        }

        private Country _country;
        public Country Country
        {
            get { return _country; }
            set
            {
                if (value != null)
                    SetProperty(ref _country, value);
            }
        }

        private State _state;
        public State State
        {
            get { return _state; }
            set
            {
                if (value != null)
                    SetProperty(ref _state, value);
            }
        }

        private string _zip = string.Empty;
        public string Zip
        {
            get { return _zip; }
            set { SetProperty(ref _zip, value); }
        }

        private string _work = string.Empty;
        public string Work
        {
            get { return _work; }
            set { SetProperty(ref _work, value); }
        }

        private string _cell = string.Empty;
        public string Cell
        {
            get { return _cell; }
            set { SetProperty(ref _cell, value); }
        }

        private string _oldPassword = string.Empty;
        public string OldPassword
        {
            get { return _oldPassword; }
            set { SetProperty(ref _oldPassword, value); }
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get { return _newPassword; }
            set { SetProperty(ref _newPassword, value); }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }

        private ObservableCollection<Country> _countryCollection;
        public ObservableCollection<Country> CountryCollection
        {
            get { return _countryCollection; }
            set { SetProperty(ref _countryCollection, value); }
        }

        private int _selectedCountryIndex;
        public int SelectedCountryIndex
        {
            get { return _selectedCountryIndex; }
            set { SetProperty(ref _selectedCountryIndex, value); }
        }

        private int _selectedStateIndex;
        public int SelectedStateIndex
        {
            get { return _selectedStateIndex; }
            set { SetProperty(ref _selectedStateIndex, value); }
        }

        private ObservableCollection<State> _stateCollection;
        public ObservableCollection<State> StateCollection
        {
            get { return _stateCollection; }
            set { SetProperty(ref _stateCollection, value); }
        }

        #endregion

        #region Commands
        public RelayCommand<object> SaveButtonClicked { get; set; }
        public RelayCommand<object> SavePasswordChangeButtonClicked { get; set; }
        public RelayCommand<object> UserProfileUILoaded { get; set; }
        public RelayCommand<object> OldPasswordChange { get; set; }
        public RelayCommand<object> NewPasswordChange { get; set; }
        public RelayCommand<object> ConfirmPasswordChange { get; set; }
        public RelayCommand<object> CountrySelectionChange { get; set; }
        #endregion

        /// <summary>
        /// User Profile ViewModel
        /// </summary>
        public UserProfileViewModel()
        {
            try
            {
                UserProfileUILoaded = new RelayCommand<object>(parameter => UserProfileUILoadedAction());
                SaveButtonClicked = new RelayCommand<object>((parameter) => SaveButtonAction());
                OldPasswordChange = new RelayCommand<object>((parameter) => SaveOldPasswordAction(parameter));
                NewPasswordChange = new RelayCommand<object>((parameter) => SaveNewPasswordAction(parameter));
                ConfirmPasswordChange = new RelayCommand<object>((parameter) => SaveConfirmPasswordAction(parameter));
                SavePasswordChangeButtonClicked = new RelayCommand<object>((parameter) => SavePasswordChangeButtonAction());
                CountrySelectionChange = new RelayCommand<object>((parameter) => BindStateBasedOnTheCountry());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindStateBasedOnTheCountry()
        {
            try
            {
                States states = GeneralManager.GetStates();
                StateCollection = new ObservableCollection<State>(states.Cast<State>().Where(state => state.CountryID == Country.CountryID));
                StateCollection.Insert(0, new State(-1, "-select-", -1));
                SelectedStateIndex = 0;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// UserProfile UI Loaded Action
        /// </summary>
        private void UserProfileUILoadedAction()
        {
            try
            {
                ClientUser user = new ClientUser();
                user = ClientManager.GetCompanyUser(UserID);
                FirstName = user.FirstName;
                LastName = user.LastName;
                _login = user.LoginName;
                Mail = user.EMail;
                AddressLine1 = user.Address1;
                AddressLine2 = user.Address2;
                Country country = GeneralManager.GetCountry(user.CountryID);
                Countries countries = GeneralManager.GetCountries();
                CountryCollection = new ObservableCollection<Country>(countries.Cast<Country>().OrderBy(x => x.Name));
                Country country1 = CountryCollection.FirstOrDefault(c => c.Name == country.Name);
                Country = country;
                SelectedCountryIndex = CountryCollection.IndexOf(country1);
                BindStateBasedOnTheCountry();
                State state1 = StateCollection.FirstOrDefault(c => c.StateID == user.StateID);
                SelectedStateIndex = StateCollection.IndexOf(state1);
                State = state1;
                Zip = user.Zip;
                Work = user.TelephoneWork;
                Cell = user.TelephoneMobile;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save Password Change Button Action
        /// </summary>
        private void SavePasswordChangeButtonAction()
        {
            try
            {
                ClientUser user = new ClientUser();
                user = ClientManager.GetCompanyUser(UserID);

                if (!NewPassword.Equals(ConfirmPassword))
                {
                    MessageBox.Show("Password does not match the confirm password.", "Prana Alert", MessageBoxButtons.OK);
                }
                else if (!PBKDF2Encryption.VerifyPassword(OldPassword, user.Password))
                {
                    MessageBox.Show("Incorrect Old Password.", "Prana Alert", MessageBoxButtons.OK);
                }
                else
                {
                    string encryptedNewPassword = PBKDF2Encryption.GetPBKDF2HashData(NewPassword);
                    string errorMsg = string.Empty;
                    using (var txscope = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 2, 0)))
                    {

                        int userID = ClientManager.SaveUserPassword(UserID, encryptedNewPassword);
                        if (userID == -1)
                        {
                            errorMsg = "User not found. Please contact administrator.";
                        }
                        else
                        {
                            errorMsg = UpdateWindowsUser(OldPassword, NewPassword);
                            if (string.IsNullOrEmpty(errorMsg))
                            {
                                txscope.Complete();
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        MessageBox.Show("Password updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(errorMsg, "Prana Alert", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Window User based on updates in Prana User
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>error message</returns>
        private string UpdateWindowsUser(string oldPassword, string newPassword)
        {
            String errorMessage = string.Empty;
            try
            {
                if (_isWindowUserReq)
                {
                    if (DirectoryEntry.Exists("WinNT://" + Environment.MachineName + "/" + _login))
                    {
                        DirectoryEntry localDirectory = new DirectoryEntry("WinNT://" + Environment.MachineName.ToString());
                        DirectoryEntries users = localDirectory.Children;
                        DirectoryEntry localUser = users.Find(_login);
                        localUser.Invoke("ChangePassword", oldPassword, newPassword);
                    }
                    else
                    {
                        MessageBox.Show("Window user doesn't exist, please add manually.", "Alert", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null || string.IsNullOrEmpty(ex.InnerException.Message))
                    errorMessage = ex.Message;
                else
                    errorMessage = ex.InnerException.Message;
            }
            return errorMessage;
        }

        /// <summary>
        /// Save Button Action
        /// </summary>
        private void SaveButtonAction()
        {
            try
            {
                ClientUser user = new ClientUser();
                bool checkValidation = true;
                string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex emailRegex = new Regex(emailCheck);
                user.UserID = UserID;
                if (!string.IsNullOrEmpty(FirstName))
                {
                    user.FirstName = FirstName;
                }
                else
                {
                    checkValidation = false;
                    MessageBox.Show("Please enter the First Name.", "Prana Alert", MessageBoxButtons.OK);
                    return;
                }
                user.LastName = LastName;
                Match emailMatch = emailRegex.Match(Mail);
                if (!string.IsNullOrEmpty(Mail) && emailMatch.Success)
                    user.EMail = Mail;
                else
                {
                    checkValidation = false;
                    MessageBox.Show("Please enter the Correct Email.", "Prana Alert", MessageBoxButtons.OK);
                    return;
                }
                if (!string.IsNullOrEmpty(AddressLine1))
                    user.Address1 = AddressLine1;
                else
                {
                    checkValidation = false;
                    MessageBox.Show("Please enter the Address.", "Prana Alert", MessageBoxButtons.OK);
                    return;
                }
                user.Address2 = AddressLine2;

                if (Country != null)
                    user.CountryID = Country.CountryID;
                else
                {
                    checkValidation = false;
                    MessageBox.Show("Please select a valid Country.", "Prana Alert", MessageBoxButtons.OK);
                    return;
                }

                if (State != null && State.StateID != -1)
                    user.StateID = State.StateID;
                else
                {
                    checkValidation = false;
                    MessageBox.Show("Please Select the State.", "Prana Alert", MessageBoxButtons.OK);
                    return;
                }

                user.Zip = Zip;

                if (!string.IsNullOrEmpty(Work))
                    user.TelephoneWork = Work;
                else
                {
                    checkValidation = false;
                    MessageBox.Show("Please enter Work Telephone.", "Prana Alert", MessageBoxButtons.OK);
                    return;
                }
                if (!string.IsNullOrEmpty(Cell))
                    user.TelephoneMobile = Cell;
                else
                {
                    checkValidation = false;
                    MessageBox.Show("Please enter Cell#.", "Prana Alert", MessageBoxButtons.OK);
                    return;
                }
                if (checkValidation)
                {
                    int userID = ClientManager.SaveUserProfile(user);
                    if (userID == -1)
                    {
                        MessageBox.Show("User not found. Please contact administrator.", "Prana Alert", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("Profile details updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Please enter the detail first.", "Prana Alert", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SaveOldPasswordAction(object parameter)
        {
            try
            {
                var passwordBox = parameter as PasswordBox;
                var password = passwordBox.Password;
                OldPassword = password;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SaveNewPasswordAction(object parameter)
        {
            try
            {
                var passwordBox = parameter as PasswordBox;
                var password = passwordBox.Password;
                NewPassword = password;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SaveConfirmPasswordAction(object parameter)
        {
            try
            {
                var passwordBox = parameter as PasswordBox;
                var password = passwordBox.Password;
                ConfirmPassword = password;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
