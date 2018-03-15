using System;
using System.ComponentModel.Composition.Hosting;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using Markdown.Xaml;
using RestSharp;
using Svg;
using YouTrackClientVS.Contracts.Interfaces.Services;
using YouTrackClientVS.Infrastructure;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.UI.Converters
{
    public class UrlToImageSourceConverter : BaseMarkupExtensionConverter, IImageManager
    {
        private static IUserInformationService _userInfoService;
        private RestClient _client;

        static UrlToImageSourceConverter()
        {
            ExportProvider provider = (ExportProvider)Application.Current.Resources[Consts.IocResource];
            _userInfoService = _userInfoService ?? provider.GetExportedValue<IUserInformationService>();
        }

        public Task<BitmapImage> DownloadImage(string url)
        {
            return GetImage(url);
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = (string)value;
            var notifier = new TaskCompletionNotifier<BitmapImage>();
            notifier.StartAsync(GetImage(url));

            return notifier;
        }

        public async Task<BitmapImage> GetImage(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    System.Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_userInfoService.ConnectionData.UserName}:{_userInfoService.ConnectionData.Password}")));

            var resp = await httpClient.GetAsync(url);
            var filetype = resp.Content.Headers.ContentType.MediaType;
            var buffer = await resp.Content.ReadAsByteArrayAsync();

            return filetype.Contains("svg", StringComparison.InvariantCultureIgnoreCase) ? GetSvgImage(buffer) : UrlToBitmap(buffer);
        }

        private BitmapImage GetSvgImage(byte[] buffer)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(buffer));
            var svgDocument = SvgDocument.Open(doc);
            using (var smallBitmap = svgDocument.Draw())
            {
                var source = BitmapToImageSource(smallBitmap);
                return source;
            }
        }

        BitmapImage UrlToBitmap(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = stream;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);

                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmapimage.Freeze();

                return bitmapimage;
            }
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
