using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class ActivityIndicatorUI : Form
    {

        private const int CP_NOCLOSE_BUTTON = 0x200;


        public ActivityIndicatorUI()
        {
            InitializeComponent();

        }

        //public ActivityIndicatorUI(string topicSubscribed)
        //{
        //    SetUp(topicSubscribed);
        //}

        //ActivityIndicatorHelper _activityHelper = null;
        //public void SetUp(string Topic)
        //{
        //    try
        //    {
        //        _activityHelper = new ActivityIndicatorHelper(this);
        //        _activityHelper.SetUp();
        //        this.TopMost = true;
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //public void Start()
        //{
        //    try
        //    {
        //        _activityHelper.StartActivityIndicator();
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //public void Stop()
        //{
        //    try
        //    {
        //        _activityHelper.StopActivityIndicator();
        //        this.Close();


        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}




        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
    }
}