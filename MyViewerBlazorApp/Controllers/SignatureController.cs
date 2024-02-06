using Microsoft.AspNetCore.Mvc;
using MyViewerBlazorApp.Components;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using TXTextControl;
using TXTextControl.Web.MVC.DocumentViewer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace MyViewerBlazorApp.Controllers
{
	[ApiController]
	[Route("signature")]
	public class SignatureController : Controller
	{
		[HttpPost("SignDocument")]
		public ActionResult SignDocument([FromBody] SignatureData signatureData)
		{
			byte[] bPDF;

			// create temporary ServerTextControl
			using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
			{
				tx.Create();

				// load the document
				tx.Load(Convert.FromBase64String(signatureData.SignedDocument.Document),
				  TXTextControl.BinaryStreamType.InternalUnicodeFormat);

				// create a certificate
				X509Certificate2 cert = new X509Certificate2("App_Data/textcontrolself.pfx", "123");

				// assign the certificate to the signature fields
				TXTextControl.SaveSettings saveSettings = new TXTextControl.SaveSettings()
				{
					CreatorApplication = "TX Text Control Blazor Sample Application",
					SignatureFields = new DigitalSignature[] {
						new TXTextControl.DigitalSignature(cert, null, "txsign")
					}
				};

				// save the document as PDF
				tx.Save("App_Data/results_" + signatureData.UniqueId + ".pdf", TXTextControl.StreamType.AdobePDF, saveSettings);
			}

			return Ok();
		}
	}
}
