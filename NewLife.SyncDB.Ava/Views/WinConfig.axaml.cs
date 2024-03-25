using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using XCode.DataAccessLayer;

namespace NewLife.SyncDB.Ava.Views;

public partial class WinConfig : Window
{
    public WinConfig()
    {
        InitializeComponent();
        
        this.btnSave.Click+= BtnSaveOnClick;
        this.btnCancel.Click += (sender, e) =>
        {
            this.Hide();    
        };

    }

    private void BtnSaveOnClick(object? sender, RoutedEventArgs e)
    {
        bool conChanage=false;

        SyncSetting.Current.ServiceName = txtServiceName.Text;
        if (SyncSetting.Current.MainConn != txtMainConn.Text)
        {
            SyncSetting.Current.MainConn = txtMainConn.Text;
            conChanage = true;
        }

        if (SyncSetting.Current.BakConn != txtBakConn.Text)
        {
            SyncSetting.Current.BakConn = txtBakConn.Text;
            conChanage = true;
        }
            
        SyncSetting.Current.ThreadCount = txtThreadCount.Text.ToInt();

        if (conChanage)
        {
            DAL.AddConnStr("BakDB", SyncSetting.Current.MainConn, null, null);
            DAL.AddConnStr("ToBakDB", SyncSetting.Current.BakConn, null, null);
        }

        SyncSetting.Current.Save();
        
        this.Hide();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        
        this.txtServiceName.Text = SyncSetting.Current.ServiceName;
        this.txtMainConn.Text = SyncSetting.Current.MainConn;
        this.txtThreadCount.Text = SyncSetting.Current.ThreadCount.ToString();
        this.txtBakConn.Text = SyncSetting.Current.BakConn;
    }
}