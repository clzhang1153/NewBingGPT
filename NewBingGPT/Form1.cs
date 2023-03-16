using Microsoft.Playwright;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace NewBingGPT;

public partial class Form1 : Form
{
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;
    private IPlaywright? _playwright;
    private WebView2? _webView;
    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        //��̬����webView2 ������CDP���ƶ˿�
        _webView = new WebView2
        {
            Visible = true,
            Dock = DockStyle.Fill,
        };

        container.Controls.Add(_webView);
        await _webView.EnsureCoreWebView2Async(await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions()
        {
            AdditionalBrowserArguments = "--remote-debugging-port=9223",
        })).ConfigureAwait(true);

        _webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested!;

        //ͨ��playwright��תbing
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.ConnectOverCDPAsync("http://localhost:9223");
        _context = _browser.Contexts[0];
        _page = _context.Pages[0];

        await _page.GotoAsync("https://www.bing.com/new");
    }

    private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        // �ڵ�ǰ Webview2 �ؼ��м��� URL
        _webView!.CoreWebView2.Navigate(e.Uri);

        // ȡ�����´���
        e.Handled = true;
    }


}
