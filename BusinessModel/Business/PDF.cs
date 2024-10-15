using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml.Linq;
using Spire.Doc;
using System.IO;
using DocumentFormat.OpenXml.Drawing;
using System.Drawing.Imaging;
using System.Drawing;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace BusinessModel
{

    /// <summary>
    /// Autor: Secretaría de Educación de Guanajuato
    /// Fecha de Creación: septiembre 2019
    /// Descripción: Clase encargarda de procesar documentos PDF para llenar formularios a partir de 
    ///         la correspondencia de pares por medio de la colección List<KeyValuePair>
    /// </summary
    public partial class PDF
    {

        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: septiembre 2019
        /// Descripción: método público estático encargardo de procesar una plantilla en docx con content controls para llenarlos con valores
        ///         mediante la correspondencia de pares por medio de la colección List<KeyValuePair>
        /// </summary
        /// <param name="rutaPlantilla">Parámetro que hace referencia al archivo docx original con formulario.</param>
        /// <param name="formsValues">Parámetro que contiene la lista de valores correspondiente a los campos de formulario que tiene el PDF</param>
        /// <param name="rutaQr">Parámetro con la url para generación del código qr</param>
        /// <returns>Devuelve un stream con la generación del archivo de salida</returns>
        /// <remarks></remarks>
        public static Stream GeneratePDFfromList(string rutaPlantilla, List<KeyValuePair<string, string>> formsValues, string rutaQr)
        {
            return ProcesarDocumentoDOCX(rutaPlantilla, formsValues, rutaQr);
        }

        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: septiembre 2019
        /// Descripción: método público estático encargardo de procesar una plantilla en docx con content controls para llenarlos con los valores de elementos y attributos
        ///         extraídos de un documento xml, se crea una lista a partir de ellos y posteriormente se busca la correspondencia de pares por medio de la colección List<KeyValuePair>
        /// </summary
        /// <param name="rutaPlantilla">Parámetro que hace referencia al archivo docx original con formulario.</param>
        /// <param name="xmldoc">Parámetro que contiene el documento xml XDocument</param>
        /// <param name="rutaQr">Parámetro con la url para generación del código qr</param>
        /// <returns>Devuelve un stream con la generación del archivo de salida</returns>
        /// <remarks></remarks>
        public static Stream GeneratePDFfromXML(string rutaPlantilla, XDocument xmldoc, string rutaQr, List<KeyValuePair<string, string>> adicionales=null)
        {
            List<KeyValuePair<string, string>> formsValues = ObtenerListfromXML(xmldoc);

            if (adicionales != null)
            {
                formsValues.AddRange(adicionales);
            }

            return ProcesarDocumentoDOCX(rutaPlantilla, formsValues, rutaQr);
        }


        // Método para obtener una lista List<KeyValuePair> de elementos y atributos contenidos en un documento xml XDocument
        public static List<KeyValuePair<string, string>> ObtenerListfromXML(XDocument xmldoc)
        {
            XElement xElement = xmldoc.Root;

            List<KeyValuePair<string, string>> listaKeysValues = new List<KeyValuePair<string, string>>();

            foreach (XElement element in xElement.DescendantsAndSelf()) // elementos del xml
            {
                string value = element.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    listaKeysValues.Add(new KeyValuePair<string, string>(element.Name.LocalName, value));
                }

                if (element.HasAttributes)
                    foreach (XAttribute attribute in element.Attributes()) // atributos del xml
                    {
                        if (!attribute.IsNamespaceDeclaration)
                        {
                            listaKeysValues.Add(new KeyValuePair<string, string>(attribute.Parent.Name.LocalName + "_" + attribute.Name.LocalName, attribute.Value));
                        }
                    }
            }

            return listaKeysValues;
        }

        // Abrimos el documento en word docx y obtenemos stream con el nuevo documento pdf
        private static Stream ProcesarDocumentoDOCX(string rutaPlantilla, List<KeyValuePair<string, string>> formsValues, string rutaQr)
        {
            if (rutaPlantilla.Contains(".docx"))
            {
                Stream streamDocumento = new MemoryStream();

                try
                {
                    ///Crear el stream que representa al documento
                    byte[] byteArray = File.ReadAllBytes(rutaPlantilla);
                    streamDocumento.Write(byteArray, 0, (int)byteArray.Length);
                }
                catch (Exception ex)
                {
                    return null;
                }
                return CrearPDFfromXMLDocx(streamDocumento, formsValues, rutaQr);
            }

            return null;

        }

        // Obtener stream con el documento pdf a partir del documento docx
        private static Stream CrearPDFfromXMLDocx(Stream streamDocumento, List<KeyValuePair<string, string>> formsValues, string rutaQr)
        {
            bool procesado = false;
            Stream streamDocumentoFinal = new MemoryStream();
            try
            {

                // Create instance of OpenSettings
                OpenSettings openSettings = new OpenSettings();

                // Add the MarkupCompatibilityProcessSettings
                openSettings.MarkupCompatibilityProcessSettings =
                                new MarkupCompatibilityProcessSettings(
                                    MarkupCompatibilityProcessMode.ProcessAllParts,
                                    DocumentFormat.OpenXml.FileFormatVersions.Office2013);

                WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(streamDocumento, true, openSettings);
                procesado = EscribirCampos(ref wordprocessingDocument, ref formsValues, ref rutaQr);
                wordprocessingDocument.Close();

                if (procesado)
                {
                    //Create a new document 
                    Spire.Doc.Document document = new Spire.Doc.Document();
                    document.LoadFromStream(streamDocumento, FileFormat.Docx2013);
                    //Save doc file to pdf.
                    ToPdfParameterList tPDF = new ToPdfParameterList();
                    tPDF.IsEmbeddedAllFonts = true;
                   document.JPEGQuality = 100;
                   
                    document.SaveToStream(streamDocumentoFinal, tPDF);
                    document.Close();
                }
                else
                {
                    streamDocumentoFinal = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            streamDocumentoFinal.Position = 0; // importante añadir ya que si no, se crea un archivo dañado
            return streamDocumentoFinal;
        }

        // Procesamos los content controls del documento docx y escribimos el texto que le corresponde de acuerdo al valor del Key y Value de la lista 'data'.
        private static bool EscribirCampos(ref WordprocessingDocument documento, ref List<KeyValuePair<string, string>> listKeysValues, ref string rutaQr)
        {
            try
            {
                ///Hacer una lista de los content controls en el encabezado, pie de página y cuerpo del documento
                List<SdtElement> listSDTElements = new List<SdtElement>();
                foreach (var head in documento.MainDocumentPart.HeaderParts)
                {
                    listSDTElements.AddRange(head.Header.Descendants<SdtElement>());
                }

                foreach (var foot in documento.MainDocumentPart.FooterParts)
                {
                    listSDTElements.AddRange(foot.Footer.Descendants<SdtElement>());
                }

                listSDTElements.AddRange(documento.MainDocumentPart.Document.Body.Descendants<SdtElement>());

                int startIndex = 0, index = 0;

                // Obtenemos lista de strings con los keys de la lista de keys y values
                List<string> dataKeys = listKeysValues.Select(el => el.Key).ToList();

                List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
                data.AddRange(listKeysValues);

                // Recorrer la lista de content controls encontrados en el documento word
                foreach (var sdtElement in listSDTElements)
                {
                    string llave = "";
                    try
                    {
                        llave = sdtElement.SdtProperties.GetFirstChild<Tag>().Val;
                    }
                    catch (Exception exx) { continue; }


                    // Si encontramos coincidencia del content control con el key en la lisa, incluimos el texto correspondiente en el pdf
                    //index = dataKeys.FindIndex(startIndex, str => str.Equals(llave));
                   
                        //startIndex = index;
                        try
                        {
                            sdtElement.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().First().Text = String.Empty;
                            sdtElement.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().First().Text = (from c in listKeysValues where c.Key==llave select c)==null?"":( from c in listKeysValues where c.Key == llave select c).FirstOrDefault().Value;
                        }
                        catch (Exception ex)
                    {
                         
                        continue;
                        }
                     
                    }
                     

                
                AgregarImagenQr(ref documento, ref rutaQr, "CodigoQR");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Autor: Secretaría de Educación de Guanajuato
        /// Fecha de Creación: septiembre 2019
        /// Descripción: método público estático encargardo de agregar código qr a un pdf a partir de un documento docx con un content control de imagen
        /// </summary
        /// <param name="documento">documento docx con content control de imagen 
        /// <param name="rutaQr">Parámetro con la url para generación del código qr</param>
        /// <param name="llaveImagen">Nombre del content control de imagen donde se colocará el código qr, se realiza un búsqueda en el documento del content control con este nombre</param>
        /// <remarks></remarks>
        public static void AgregarImagenQr(ref WordprocessingDocument documento, ref string rutaQr, string llaveImagen)
        {
            if (!string.IsNullOrEmpty(rutaQr))
            {
                try
                {
                    List<SdtElement> descendants = documento.MainDocumentPart.Document.Descendants<SdtElement>().ToList();
                    foreach (HeaderPart headerPart in documento.MainDocumentPart.HeaderParts)
                    {
                        descendants.AddRange(headerPart.Header.Descendants<SdtElement>().ToList());
                    }
                    foreach (var footerPart in documento.MainDocumentPart.FooterParts)
                    {
                        descendants.AddRange(footerPart.Footer.Descendants<SdtElement>().ToList());
                    }


                    var ccc =
                        descendants.FirstOrDefault(c =>
                        {
                            SdtProperties p = c.Elements<SdtProperties>().FirstOrDefault();
                            if (p != null)
                            {
                                SdtContentPicture pict = p.Elements<SdtContentPicture>().FirstOrDefault();

                                SdtAlias a = p.Elements<SdtAlias>().FirstOrDefault();
                                if (pict != null && a.Val == llaveImagen)
                                {
                                    return true;
                                }
                            }
                            return false;
                        });

                    string embedRef = null;
                    if (ccc != null)
                    {
                        Drawing dr = ccc.Descendants<Drawing>().FirstOrDefault();
                        if (dr != null)
                        {
                            Blip blip = dr.Descendants<Blip>().FirstOrDefault();
                            if (blip != null)
                                embedRef = blip.Embed;
                        }
                    }

                    if (embedRef != null)
                    {
                        IdPartPair idpp = documento.MainDocumentPart.Parts.Where(pa => pa.RelationshipId == embedRef).FirstOrDefault();
                        if (idpp != null)
                        {
                            // Generación del código qr
                            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                            var qrCode = qrEncoder.Encode(rutaQr);
                            var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                            var qr = new MemoryStream();

                            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, qr);
                            qr.Position = 0;
                            
                            idpp.OpenXmlPart.FeedData(qr);
                            qr.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                }
            }

        }

    }

}
