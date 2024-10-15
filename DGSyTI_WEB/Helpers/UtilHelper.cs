using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Helpers
{
    public static class UtilHelper
    {
        public const string FormatoFechaServer = "yyyy/MM/dd";
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm:ss";
        public const string FormatoHora = "HH:mm:ss";
        public const string FormatoHoraCorto = "HH:mm";
        public const string MENSAJE_TABLA_VACIA = "No hay elementos para mostrar.";
        public const string MENSAJE_RESPONSABLE_INVOLUCRADO = "[Presunto involucrado o de rango menor a presunto involucrado]";
        public const string MENSAJE_ACCION_NO_PERMITIDA = "La acción que intentó llevar a cabo no le está permitida.";
        public const string MENSAJE_ELEMENTO_NO_ENCONTRADO = "El elemento solicitado no ha sido encontrado.";
        public const string MENSAJE_PROBLEMA = "Ocurrió un problema durante el procesamiento de la petición";
        public const string MENSAJE_PETICION_INVALIDA = "La petición o alguno de sus parámetros no es válida.";
        public const string MENSAJE_SERVICIO_TEMPORALMENTE_NO_DISPONIBLE = "El servidor no se encuentra disponible en este momento. Intente nuevamente más tarde.";
        public const string MENSAJE_ELEMENTO_DUPLICADO = "Ya existe un elemento con ese nombre. Cámbielo.";

        public const string MENSAJE_RESTRICCION_INFORMACION_AMBITOS = "La información plasmada en el documento depende del usuario que lo generó.";

        /* public static Font TitleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
         public static Font SubtitleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
         public static Font DataFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
         public static Font FilterNameFont = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLDITALIC);
         public static Font FilterValueFont = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
         public static Font NoteFont = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD);

         private const int STRONG_RED_COLOR = 198;
         private const int STRONG_GREEN_COLOR = 214;
         private const int STRONG_BLUE_COLOR = 255;
         private const int LIGHT_RED_COLOR = 230;
         private const int LIGHT_GREEN_COLOR = 237;
         private const int LIGHT_BLUE_COLOR = 255;

         public static BaseColor StrongBlueBaseColor = new BaseColor(STRONG_RED_COLOR, STRONG_GREEN_COLOR, STRONG_BLUE_COLOR);
         public static BaseColor LightBlueBaseColor = new BaseColor(LIGHT_RED_COLOR, LIGHT_GREEN_COLOR, LIGHT_BLUE_COLOR);
         public static System.Drawing.Color StrongBlue = System.Drawing.Color.FromArgb(STRONG_RED_COLOR, STRONG_GREEN_COLOR, STRONG_BLUE_COLOR);
         public static System.Drawing.Color LightBlue = System.Drawing.Color.FromArgb(LIGHT_RED_COLOR, LIGHT_GREEN_COLOR, LIGHT_BLUE_COLOR);
          */
        /// <summary>
        /// Convierte el código de estatus en una cadena de texto
        /// </summary>
        /// <param name="estatusInt">Código de estatus</param>
        /// <returns>Cadena de texto con el nombre del estatus</returns>
      /*  public static string estatusStringFromInt(int estatusInt)
        {
            string strEstatus;
            switch (estatusInt)
            {
                case (int)Estatus.Activo:
                    strEstatus = Estatus.Activo.ToString();
                    break;
                case (int)Estatus.Inactivo:
                    strEstatus = Estatus.Inactivo.ToString();
                    break;
                case (int)Estatus.Eliminado:
                    strEstatus = Estatus.Eliminado.ToString();
                    break;
                default:
                    strEstatus = Estatus.Indeterminado.ToString();
                    break;
            }
            return strEstatus;
        }
        */

        /// <summary>
        /// Devuelve la imagen correspondiente al estatus del elemento
        /// </summary>
        /// <param name="estatusInt">Código de estatus</param>
        /// <returns>Ruta URL de la imagen correspondiente</returns>
     /*   public static MvcHtmlString estatusImageFromInt(int estatusInt)
        {
            string img = "";
            switch (estatusInt)
            {
                case (int)Estatus.Activo:
                    img = VirtualPathUtility.ToAbsolute("~/Images/iconos/activo.png"); ;
                    break;
                case (int)Estatus.Inactivo:
                    img = VirtualPathUtility.ToAbsolute("~/Images/iconos/cancelado.png"); ;
                    break;
                case (int)Estatus.Eliminado:
                    img = VirtualPathUtility.ToAbsolute("~/Images/iconos/rechazado.png"); ;
                    break;
                default:
                    img = VirtualPathUtility.ToAbsolute("~/Images/iconos/finalizado.png"); ;
                    break;
            }
            TagBuilder builder = new TagBuilder("estatusImage");
            builder.InnerHtml = "<img src='" + img + "' border='0' height='20' width='20'>";
            return new MvcHtmlString(builder.ToString());
        }
        */
        /// <summary>
        /// Construye el encabezado para exportar los reportes ejecutivos a PDF
        /// </summary>
        /// <param name="titulo">Titulo del reporte</param>
        /// <returns>Objeto de tipo PdfPTable</returns>
       /* public static PdfPTable BuildEncabezadoReporte(string titulo)
        {
            return BuildEncabezadoReporte(titulo, DateTime.Now, null);
        }
        */

        /// <summary>
        /// Construye el encabezado para exportar los reportes ejecutivos a PDF
        /// </summary>
        /// <param name="titulo">Titulo del reporte</param>
        /// <param name="fecha">Fecha de impresión del reporte</param>
        /// <param name="usuario">Usuario que generó el reporte</param>
        /// <returns>Objeto de tipo PdfPTable</returns>
       /* public static PdfPTable BuildEncabezadoReporte(string titulo, DateTime fecha, string usuario)
        {
            //Configuración de la sección
            PdfPTable tEncabezado = new PdfPTable(2);
            tEncabezado.SpacingAfter = 15;
            tEncabezado.SetWidthPercentage(new float[] { 125, 535 }, PageSize.A4);

            //Datos de la sección
            Image image = null;
            PdfPCell cLogo = null;
            try
            {
                Uri imageUrl = new Uri(ConfiguracionBLL.FindByLlave(Configuraciones.RutaLogoReportes.ToString()).Valor);
                image = iTextSharp.text.Image.GetInstance(imageUrl);
                image.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                image.ScaleToFit(102, 68);
            }
            catch (Exception) { }
            if (image == null)
            {
                cLogo = new PdfPCell(new Phrase(""));
            }
            else
            {
                cLogo = new PdfPCell(image);
            }
            cLogo.Border = PdfPCell.NO_BORDER;
            cLogo.Rowspan = 4;
            cLogo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

            //Nombre del sistema [primer encabezado]
            Phrase pNombreSistema = new Phrase(ConfiguracionBLL.FindByLlave(Configuraciones.NombreSistema.ToString()).Valor, TitleFont);
            PdfPCell cNombreSistema = new PdfPCell(pNombreSistema);
            cNombreSistema.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cNombreSistema.BackgroundColor = StrongBlueBaseColor;

            //Titulo del reporte
            Phrase pTitulo = new Phrase(titulo, SubtitleFont);
            PdfPCell cTitulo = new PdfPCell(pTitulo);
            cTitulo.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cTitulo.BackgroundColor = LightBlueBaseColor;

            //Usuario del reporte [si lo tiene]
            Phrase pUsuario = new Phrase(usuario == null ? "" : usuario, DataFont);
            PdfPCell cUsuario = new PdfPCell(pUsuario);
            cUsuario.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            cUsuario.BackgroundColor = LightBlueBaseColor;

            //Fecha del reporte
            Phrase pFecha = new Phrase(fecha.ToString(FormatoFechaHora), DataFont);
            PdfPCell cFecha = new PdfPCell(pFecha);
            cFecha.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            cFecha.BackgroundColor = LightBlueBaseColor;

            //Adición de los datos a la tabla
            tEncabezado.AddCell(cLogo);
            tEncabezado.AddCell(cNombreSistema);
            tEncabezado.AddCell(cTitulo);
            tEncabezado.AddCell(cUsuario);
            tEncabezado.AddCell(cFecha);

            //Devolución del objeto
            return tEncabezado;
        }
        */
        /// <summary>
        /// Construye una tabla con la información de los filtros seleccionados por el usuario
        /// </summary>
        /// <param name="filtros">Listado de filtros</param>
        /// <returns>Objeto de tipo PdfPTable</returns>
       /* public static PdfPTable BuildFilterSection(List<KeyValuePair<string, string>> filtros)
        {
            //Configuración de la sección
            PdfPTable tFiltros = new PdfPTable(4);
            tFiltros.SpacingAfter = 15;
            //tFiltros.SpacingBefore = 15;
            tFiltros.SetWidthPercentage(new float[] { 110, 220, 110, 220 }, PageSize.A4);

            //Datos de la sección
            //Explicación de los datos mostrados
            Phrase pDescripción = new Phrase("Filtros especificados para la información mostrada", DataFont);
            PdfPCell cDescripcion = new PdfPCell(pDescripción);
            cDescripcion.Colspan = 4;
            cDescripcion.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cDescripcion.BackgroundColor = StrongBlueBaseColor;
            //Añadimos la celda de explicación
            tFiltros.AddCell(cDescripcion);

            int filtrosAgregados = 0;
            foreach (KeyValuePair<string, string> par in filtros)
            {
                //Nombre del filtro
                Phrase pNombreFiltro = new Phrase(par.Key, FilterNameFont);
                PdfPCell cNombreFiltro = new PdfPCell(pNombreFiltro);
                cNombreFiltro.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cNombreFiltro.BackgroundColor = LightBlueBaseColor;
                tFiltros.AddCell(cNombreFiltro);

                //Valor del filtro
                Phrase pValorFiltro = new Phrase(par.Value, FilterValueFont);
                PdfPCell cValorFiltro = new PdfPCell(pValorFiltro);
                cValorFiltro.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tFiltros.AddCell(cValorFiltro);

                filtrosAgregados++;
                if (filtrosAgregados == 2)
                {
                    filtrosAgregados = 0;
                }
            }

            int celdasPendientes = 2 - filtrosAgregados;
            if (celdasPendientes > 0 && celdasPendientes < 2)
            {
                for (int i = 0; i < celdasPendientes; i++)
                {
                    tFiltros.AddCell(""); //Filtro nombre
                    tFiltros.AddCell(""); //Filtro valor
                }
            }

            //Agregamos la anotación que indica que la información plasmada en un reporte depende del ámbito del usuario que la genera
            Phrase pNota = new Phrase(MENSAJE_RESTRICCION_INFORMACION_AMBITOS, NoteFont);
            PdfPCell cNota = new PdfPCell(pNota);
            cNota.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cNota.BackgroundColor = StrongBlueBaseColor;
            cNota.Colspan = 4;
            tFiltros.AddCell(cNota);

            //Devolución del objeto
            return tFiltros;
        }
        */
        /// <summary>
        /// Construye un objeto de tipo KeyValuePair de acuerdo a los filtros seleccionados 
        /// por el usuario que generó el reporte
        /// </summary>
        /// <param name="sheet">Objeto ExcelWorksheet que se exportará a Excel</param>
        /// <param name="filtros">Litsado de filtros</param>
        /// <param name="nColumnas">Número de columnas del archivo de Excel</param>
        /// <param name="startingRow">Índice inicial de fila</param>
        /// <param name="startingCol">Índice inicial de columna</param>
        /// <returns>KeyValuePair</returns>
     /*   public static KeyValuePair<int, int> BuildFilterSection(ExcelWorksheet sheet, List<KeyValuePair<string, string>> filtros, int nColumnas, int startingRow, int startingCol)
        {
            int nFilaInicial = startingRow;
            int nColumnaInicial = startingCol;
            int nFilaActual = startingRow;
            int nColumnaActual = startingCol;

            int filterCounter = 0;
            foreach (KeyValuePair<string, string> filtro in filtros)
            {
                //Nombre del filtro
                sheet.Cells[nFilaActual, nColumnaActual].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[nFilaActual, nColumnaActual].Style.Font.Bold = true;
                sheet.Cells[nFilaActual, nColumnaActual].Style.Font.Italic = true;
                sheet.Cells[nFilaActual, nColumnaActual].Style.Fill.BackgroundColor.SetColor(LightBlue);
                sheet.Cells[nFilaActual, nColumnaActual].Value = filtro.Key;
                nColumnaActual++;

                //Valor del filtro
                if (filtro.Key == "Estatus:")
                {
                    var estatuses = filtro.Value.Split(',');
                    foreach (var est in estatuses)
                    {
                        sheet.Cells[nFilaActual, nColumnaActual++].Value = est;
                    }
                }
                else
                {
                    sheet.Cells[nFilaActual, nColumnaActual++].Value = filtro.Value;
                }
                filterCounter++;

                if (filterCounter == nColumnas)
                {
                    filterCounter = 0;
                    nFilaActual++;
                    nColumnaActual = nColumnaInicial;
                }
            }
            sheet.Cells[++nFilaActual, nColumnaInicial].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[nFilaActual, nColumnaInicial].Style.Font.Bold = true;
            sheet.Cells[nFilaActual, nColumnaInicial].Style.Fill.BackgroundColor.SetColor(StrongBlue);
            sheet.Cells[nFilaActual, nColumnaInicial].Value = MENSAJE_RESTRICCION_INFORMACION_AMBITOS;
            sheet.Cells[nFilaActual, nColumnaInicial, nFilaActual, nColumnaInicial + (2 * nColumnas) - 1].Merge = true;

            return new KeyValuePair<int, int>(nFilaActual, nColumnaInicial + (2 * nColumnas) - 1);
        }

        public static PdfPTable BuildKeyValueTable(string tableTitle, Dictionary<string, int> datos, List<string> llaves)
        {
            PdfPTable tData = new PdfPTable(2);
            tData.SpacingAfter = 15;
            tData.SetWidthPercentage(new float[] { 220, 440 }, PageSize.A4);

            Phrase pTitulo = new Phrase(tableTitle, UtilidadesGenerales.SubtitleFont);
            PdfPCell cTitulo = new PdfPCell(pTitulo);
            cTitulo.Colspan = 2;
            cTitulo.BackgroundColor = StrongBlueBaseColor;
            tData.AddCell(cTitulo);

            bool alternateBackColor = false;
            foreach (string key in llaves)
            {
                Phrase pKey = new Phrase(key, UtilidadesGenerales.DataFont);
                PdfPCell cKey = new PdfPCell(pKey);
                cKey.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                if (alternateBackColor)
                {
                    cKey.BackgroundColor = LightBlueBaseColor;
                }
                tData.AddCell(cKey);


                Phrase pValue = new Phrase(datos[key].ToString(), UtilidadesGenerales.DataFont);
                PdfPCell cValue = new PdfPCell(pValue);
                cValue.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                if (alternateBackColor)
                {
                    cValue.BackgroundColor = LightBlueBaseColor;
                }
                tData.AddCell(cValue);
                alternateBackColor = !alternateBackColor;
            }
            return tData;
        }

        public static KeyValuePair<int, int> BuildKeyValueTable(ExcelWorksheet sheet, string tableTitle, Dictionary<string, int> datos, List<string> llaves, int startingRow, int startingCol)
        {
            int renglon = startingRow;
            int columna = startingCol;

            sheet.Cells[renglon, columna].Value = tableTitle;
            sheet.Cells[renglon, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[renglon, columna].Style.Fill.BackgroundColor.SetColor(UtilidadesGenerales.StrongBlue);
            sheet.Cells[renglon, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[renglon, columna, renglon, columna + 1].Merge = true;

            foreach (string key in llaves)
            {
                columna = startingCol;

                sheet.Cells[++renglon, columna].Value = key;
                sheet.Cells[renglon, ++columna].Value = datos[key];
            }
            return new KeyValuePair<int, int>(renglon, startingCol + 1);
        }

        /// <summary>
        /// Devuelve el listado de sexos configurados en el enumerado de Genero de involucrado
        /// </summary>
        /// <returns></returns>
        public static List<SexoInvolucrado> SexosList()
        {
            return SexosList(false);
        }
        /// <summary>
        /// Devuelve el listado de sexos configurados en el enumerado de Genero de involucrado, 
        /// agrega también un elemento al final de la lista que permitirá al usuario seleccionar ambos sexos
        /// </summary>
        /// <param name="includeAmbos">Determina si se agrega un elemento al final de la lista con ambos sexos</param>
        /// <returns></returns>
        public static List<SexoInvolucrado> SexosList(bool includeAmbos)
        {
            List<SexoInvolucrado> sexosList = new List<SexoInvolucrado>();
            foreach (SexoInvolucrado sex in Enum.GetValues(typeof(SexoInvolucrado)))
            {
                if ((sex == SexoInvolucrado.Ambos && includeAmbos) || (sex != SexoInvolucrado.Ambos))
                {
                    sexosList.Add(sex);
                }
            }
            sexosList.Sort();
            return sexosList;
        }

        /// <summary>
        /// Devuelve el listado de sexos configurados en el enumerado de Genero de involucrado
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> SexosListItem()
        {
            return SexosListItem(false);
        }

        /// <summary>
        /// Devuelve el listado de sexos configurados en el enumerado de Genero de involucrado
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> SexosListItem(bool includeAmbos)
        {
            List<SelectListItem> sexos = new List<SelectListItem>();
            foreach (SexoInvolucrado sexo in SexosList(includeAmbos))
            {
                sexos.Add(new SelectListItem
                {
                    Text = sexo.ToString(),
                    Value = ((int)sexo).ToString()
                });
            }
            return sexos;
        }

        public static BitacoraML BuildEntradaBitacora(string tipoProceso, string procedimiento, string accion, string usuario, string ip, string host, string pantalla)
        {
            try
            {
                var bitacora = new Bitacora()
                {
                    TipoProceso = tipoProceso,
                    Procedimiento = procedimiento,
                    Accion = accion,
                    User = usuario,
                    Ip = ip,
                    Maquina = host,
                    Pantalla = pantalla
                };
                return bitacora;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Clase auxiliar que nos ayuda a validar la extensión del archivo que sube el usuario al sistema
    /// </summary>
    public class HttpPostedFileExtensionAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly FileExtensionsAttribute _fileExtensionsAttribute = new FileExtensionsAttribute();

        public HttpPostedFileExtensionAttribute()
        {
            ErrorMessage = _fileExtensionsAttribute.ErrorMessage;
        }

        public string Extensions
        {
            get { return _fileExtensionsAttribute.Extensions; }
            set { _fileExtensionsAttribute.Extensions = value; }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "extension",
                ErrorMessage = this.ErrorMessage
            };

            rule.ValidationParameters["extension"] =
                _fileExtensionsAttribute.Extensions
                    .Replace(" ", string.Empty).Replace(".", string.Empty)
                    .ToLowerInvariant();

            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return _fileExtensionsAttribute.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            return _fileExtensionsAttribute.IsValid(file != null ? file.FileName : value);
        }*/
    }
}