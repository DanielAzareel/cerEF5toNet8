using BusinessModel.Business.JavaScience;
using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Models.Personalizados;
using BusinessModel.Persistence.Titulos;
using BusinessModel.ServiceAccess;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BusinessModel.Business
{
    public class CertificadoBL
    {
        //GENERA LOS TABULADORES DE LA VISTA PRINCIPAL.
        public List<TabML> GenerarTabs(String[] arrayTablasFiltro, bool[] arrrayAccionesTab, bool bFirmaInstitucion)
        {
            List<TabML> returnListTaps = new List<TabML>();
            TabML oTap = new TabML();

            if (bFirmaInstitucion)
            {
                // Creamos el tab Sellado institucional
                if (arrrayAccionesTab[0])
                {
                    oTap.label = "Sellado institucional";
                    oTap.activa = true;
                    oTap.controller = "Titulo";
                    oTap.metodo = "SelloInstitucional";
                    oTap.parametro = "";
                    oTap.idTablaFiltrado = arrayTablasFiltro[0];
                    returnListTaps.Add(oTap);
                }
            }

            // Creamos el tab Sellado concentradora
            if (arrrayAccionesTab[1])
            {
                oTap = new TabML();
                oTap.label = "Sellado concentradora";
                oTap.activa = true;
                oTap.controller = "Titulo";
                oTap.metodo = "SelloConcentradora";
                oTap.parametro = "";
                oTap.idTablaFiltrado = arrayTablasFiltro[1];
                returnListTaps.Add(oTap);
            }

            // Creamos el tab Proceso SEP
            if (arrrayAccionesTab[2])
            {
                oTap = new TabML();
                oTap.label = "Proceso SEP";
                oTap.activa = true;
                oTap.controller = "Titulo";
                oTap.metodo = "SolicitudesEnProceso";
                oTap.parametro = "";
                oTap.idTablaFiltrado = arrayTablasFiltro[2];
                returnListTaps.Add(oTap);
            }

            // Creamos el tab Concluidos
            if (arrrayAccionesTab[3])
            {
                oTap = new TabML();
                oTap.label = "Concluidos";
                oTap.activa = true;
                oTap.controller = "Titulo";
                oTap.metodo = "SolicitudesFinalizadas";
                oTap.parametro = "";
                oTap.idTablaFiltrado = arrayTablasFiltro[3];
                returnListTaps.Add(oTap);
            }

            return returnListTaps;
        }

        #region Sellado institucional
        //CREA EL MODELO REQUERIDO PARA EL TAB DE INSTITUCION.
        public SelladoInstitucionML CrearModeloSelladoInstitucion(criteriosBusquedaTitulosML filtros, int pagina, int bloque)
        {
            SelladoInstitucionML oSelladoInstitucionML = new SelladoInstitucionML();
            oSelladoInstitucionML.oTituloML = getListadoTitulos(filtros, pagina, bloque);
            return oSelladoInstitucionML;
        }

        //SELLADO DE INSTITUCION
        public String[] SellarDocumentosInstitucion(byte[] archivoKey, String nameContrasenia, criteriosBusquedaTitulosML criterios, String pUsuario, String idConcentradora, String idInstitucion, PerfilML perfilActivo)
        {
            String[] mensajeArray = new String[2];
            mensajeArray[0] = "false";
            mensajeArray[1] = "";

            //Almacena las acciones que se realizaron o intentaron ejecutar.
            List<BitacoraML> listBitacora = new List<BitacoraML>();
            BitacoraML bitacora = new BitacoraML();

            //Se obtienen los documentos buscados por perfil seleccionado. 
            List<titDocumento> listaDocumentosInstitucion = new TitDocumentoDAL().ObtenerListaDocumentosFiltros(criterios).ToList();
            List<titFirmaResponsable> listTitFirmaResponsables = new TitFirmaResponsableDAL().ObtenerFirmantesByDocId(listaDocumentosInstitucion.Select(d => d.docId).ToList());
            listaDocumentosInstitucion.ForEach(d => d.titFirmaResponsable = (from firma in listTitFirmaResponsables where firma.docId == d.docId select firma).ToList());

            if (listaDocumentosInstitucion.Count != 0)
            {
                //Listas para almacenar los registros del sellado.
                List<titFirmaResponsable> listaResponsablesSelladores = new List<titFirmaResponsable>();
                List<titDocumento> listaDocumentosSellados = new List<titDocumento>();

                foreach (titDocumento documento in listaDocumentosInstitucion)
                {
                    //Obtiene el firmante del documento.
                    titFirmaResponsable firmante = new titFirmaResponsable();
                    firmante = documento.titFirmaResponsable.Where(fir => fir.firConcentradora == false).FirstOrDefault();

                    if (firmante != null)
                    {
                        try
                        {
                            //Generacion del la cadena original y sellado.
                            TituloElectronico tituloElectronico = LlenarObjetoTituloElectronico(documento);

                            //Certificado para validar.
                            byte[] bytesCertificado = Encoding.UTF8.GetBytes(firmante.firCertificadoResponsable);

                            X509Certificate2 certificado = new X509Certificate2(bytesCertificado);
                            firmante.firCurp = ObtenerCurpCertificado(certificado);
                            String cadenaOriginal = GenerarCadenaOriginal(tituloElectronico, firmante);
                            String selloCadenaOriginal = GenerarSello(cadenaOriginal, archivoKey, nameContrasenia);

                            //Validacion del archivo key con el archivo cer.
                            bool banderaValidacion = ValidarSellado(cadenaOriginal, selloCadenaOriginal, certificado);

                            //En caso de ser valido el .key con .cer
                            if (banderaValidacion)
                            {
                                //Complementar datos del regitro de la tabla titFirmaResponsable.
                                firmante.firCurp = ObtenerCurpCertificado(certificado);
                                firmante.firSello = selloCadenaOriginal;
                                firmante.firNoCertificadoResponsable = certificado.SerialNumber;
                                firmante.firCadenaOriginal = cadenaOriginal;

                                //El documento pasa a estar en estatus sellado por institucion.
                                documento.estDocumentoId = 2;

                                //Agregar a las listas los documentos sellados y sus firmantes.
                                listaDocumentosSellados.Add(documento);
                                listaResponsablesSelladores.Add(firmante);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }

                //Guardar registros si existieron documentos sellados.
                if (listaResponsablesSelladores.Count > 0)
                {
                    List<DocumentoSolicitud> listDocumentoSolicitud = new List<DocumentoSolicitud>();

                    //Configuración de total de solicitudes y registros por solicitud.
                    int maxDocumentos = 5000;
                    int totalDocumentos = listaDocumentosSellados.Count;
                    int totalSolicitudes = (totalDocumentos > maxDocumentos) ?
                        ((totalDocumentos % maxDocumentos) > 0 ? (totalDocumentos / maxDocumentos) + 1 : (totalDocumentos / maxDocumentos)) : 1;
                    int tamanioBloque = (totalDocumentos / totalSolicitudes);
                    DateTime fechaActual = DateTime.Now;

                    for (int i = 0, j = 0, tamBloqueFinal = 0; i <= totalSolicitudes; i++, j += tamBloqueFinal, totalSolicitudes--)
                    {
                        //Crear la nueva solicitud.
                        titSolicitud nuevaSolicitud = new titSolicitud
                        {
                            solId = Guid.NewGuid().ToString(),
                            solFechaSelladoInstitucion = fechaActual,
                            estSolicitudId = 1, //1 = Sellado por institucion.
                            conId = criterios.concentradora
                        };

                        //Se define el tamaño del bloque.
                        tamBloqueFinal = tamanioBloque + (listaDocumentosSellados.Skip(j).Take(totalDocumentos).Count() % totalSolicitudes > 0 ? 1 : 0);
                        List<titDocumento> bloqueDocumentos = listaDocumentosSellados.Skip(j).Take(tamBloqueFinal).ToList();

                        listDocumentoSolicitud.Add(DocumentosPorSolicitud(nuevaSolicitud, bloqueDocumentos));
                    }

                    //Se crea Datatable de registros sellados para la bitacora.
                    DataTable dataTableBitacora = DataTableBitacoraSolicitudDocumento(listDocumentoSolicitud, pUsuario, "selloInstitucional", idConcentradora, idInstitucion);
                    DataTable dataTableDocumentos = DataTableDocumentos(listDocumentoSolicitud);
                    DataTable dataTableFirmantes = DataTableFirmantes(listaResponsablesSelladores);

                    //Registrar solicitud, cambiar estatus documentos y actualizar firmante.
                    bool banderaFinalizarSello = new TitSolicitudDAL().RegistrarSolicitud(listDocumentoSolicitud.Select(ds => ds.solicitudDoc).ToList(), dataTableDocumentos, dataTableFirmantes, dataTableBitacora);

                    if (banderaFinalizarSello)
                    {
                        mensajeArray[0] = "true";
                        mensajeArray[1] = (totalDocumentos > 1) ? "Se han firmado la cantidad de " + totalDocumentos.ToString() + " documentos." : "Se ha firmado 1 documento.";
                    }
                    else
                    {
                        listBitacora.Add(new BitacoraML()
                        {
                            bitId = Guid.NewGuid().ToString(),
                            bitFecha = DateTime.Now,
                            bitUsuario = pUsuario,
                            bitDescripcion = "Error del sistema.",
                            bitExitoso = false,
                            accId = "selloInstitucional"
                        });

                        DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                        StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                        mensajeArray[1] = "Ha ocurrido un error, por favor intente nuevamente.";
                    }
                }
                else
                {
                    listBitacora.Add(new BitacoraML()
                    {
                        bitId = Guid.NewGuid().ToString(),
                        bitFecha = DateTime.Now,
                        bitUsuario = pUsuario,
                        bitDescripcion = "No existen registros asociados al archivo key insertado.",
                        bitExitoso = false,
                        accId = "selloInstitucional"
                    });

                    DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                    StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                    mensajeArray[1] = "No existen registros asociados al archivo key insertado.";
                }
            }
            else
            {
                listBitacora.Add(new BitacoraML()
                {
                    bitId = Guid.NewGuid().ToString(),
                    bitFecha = DateTime.Now,
                    bitUsuario = pUsuario,
                    bitDescripcion = "No existen registros asociados al archivo key insertado.",
                    bitExitoso = false,
                    accId = "selloInstitucional"
                });

                DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                mensajeArray[1] = "No existen registros asociados al archivo key insertado.";
            }

            return mensajeArray;
        }

        #endregion

        #region Sellado concentradora
        //CREA EL MODELO REQUERIDO PARA EL TAB DE CONCENTRADORA.
        public SelladoConcentradoraML CrearModeloSelladoConcentradora(criteriosBusquedaSolicitudML filtros, int pagina, int bloque)
        {
            SelladoConcentradoraML oSelladoConcentradoraML = new SelladoConcentradoraML();
            oSelladoConcentradoraML.oSolicitudML = getListadoSolicitudes(filtros, pagina, bloque);

            return oSelladoConcentradoraML;
        }

        //OBTIENE UN LISTADO DE VIEWS DOCUMENTOS POR SOLICITUD.
        public SelladoConcentradoraML ObtenerListaViewDocumentosSolicitud(criteriosBusquedaTitulosML filtros, int pagina, int bloque)
        {
            SelladoConcentradoraML oSelladoConcentradoraML = new SelladoConcentradoraML();
            oSelladoConcentradoraML.oTituloML.listadoTitulo = new TitDocumentoDAL().ObtenerListaDocumentosInstitucion(filtros, pagina, bloque);

            return oSelladoConcentradoraML;
        }

        //SELLADO POR SOLICITUD
        public String[] SellarSolicitudConcentradora(byte[] archivoKey, String contrasenia, String IdSolicitud, criteriosBusquedaTitulosML criterios, String pUsuario, String idConcentradora, String idInstitucion, PerfilML perfilActivo)
        {
            String[] mensajeArray = new String[2];
            mensajeArray[0] = "false";
            mensajeArray[1] = "";

            //Almacena las acciones que se realizaron o intentaron ejecutar.
            List<BitacoraML> listBitacora = new List<BitacoraML>();

            //Se obtienen los documentos buscados por perfil seleccionado. 
            List<titDocumento> listaDocumentosConcentradora = new TitDocumentoDAL().ObtenerListaDocumentosFiltros(criterios);
            List<titFirmaResponsable> listTitFirmaResponsables = new TitFirmaResponsableDAL().ObtenerFirmantesByDocId(listaDocumentosConcentradora.Select(d => d.docId).ToList());
            listaDocumentosConcentradora.ForEach(d => d.titFirmaResponsable = (from firma in listTitFirmaResponsables where firma.docId == d.docId select firma).ToList());

            if (listaDocumentosConcentradora.Count != 0)
            {
                //Listas para almacenar los registros del sellado.
                List<titFirmaResponsable> listaResponsablesSelladores = new List<titFirmaResponsable>();
                List<titDocumento> listaDocumentosSellados = new List<titDocumento>();

                foreach (titDocumento documento in listaDocumentosConcentradora)
                {
                    //Obtiene el firmante del documento.
                    titFirmaResponsable firmante = new titFirmaResponsable();
                    firmante = documento.titFirmaResponsable.Where(fir => fir.firConcentradora == true).FirstOrDefault();

                    if (firmante != null)
                    {
                        try
                        {
                            //Generacion del la cadena original y sellado.
                            TituloElectronico tituloElectronico = LlenarObjetoTituloElectronico(documento);

                            //Certificado para validar.
                            byte[] bytesCertificado = Encoding.UTF8.GetBytes(firmante.firCertificadoResponsable);
                            X509Certificate2 certificado = new X509Certificate2(bytesCertificado);

                            //Complementar datos del regitro de la tabla titFirmaResponsable.
                            firmante.firCurp = ObtenerCurpCertificado(certificado);
                            String cadenaOriginal = GenerarCadenaOriginal(tituloElectronico, firmante);
                            String selloCadenaOriginal = GenerarSello(cadenaOriginal, archivoKey, contrasenia);

                            //Validacion del archivo key con el archivo cer.
                            bool banderaValidacion = ValidarSellado(cadenaOriginal, selloCadenaOriginal, certificado);

                            //En caso de ser valido el .key con .cer
                            if (banderaValidacion)
                            {
                                firmante.firSello = selloCadenaOriginal;
                                firmante.firNoCertificadoResponsable = certificado.SerialNumber;
                                firmante.firCadenaOriginal = cadenaOriginal;

                                //El documento pasa a estar en estatus sellado por concentradora.
                                documento.estDocumentoId = 3;

                                //Agregar a las listas los documentos sellados y sus firmantes.
                                listaDocumentosSellados.Add(documento);
                                listaResponsablesSelladores.Add(firmante);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }

                //Guardar registros si existieron documentos sellados.
                if (listaResponsablesSelladores.Count > 0)
                {
                    bool banderaFinalizarSello = false;
                    int documentosSellados = listaDocumentosSellados.Count;
                    int documentosSolicitud = listaDocumentosConcentradora.Count;

                    List<DocumentoSolicitud> listDocumentoSolicitud = new List<DocumentoSolicitud>();

                    //Revisa si se sellaron todos los documentos.
                    if (documentosSellados == documentosSolicitud)
                    {
                        //Actualizar estatus de la solicitud.
                        titSolicitud solicitud = new TitSolicitudDAL().ObtenerRegistroSolicitud(IdSolicitud);
                        solicitud.estSolicitudId = 2; //La solicitud pasa a estatus sellado por concentradora.
                        solicitud.solFechaSelladoConcentradora = DateTime.Now;

                        listDocumentoSolicitud.Add(DocumentosPorSolicitud(solicitud, listaDocumentosSellados));

                        //Se crean los Datatable.
                        DataTable dataTableBitacora = DataTableBitacoraSolicitudDocumento(listDocumentoSolicitud, pUsuario, "selloConcentradora", idConcentradora, "");
                        DataTable dataTableDocumentos = DataTableDocumentos(listDocumentoSolicitud);
                        DataTable dataTableFirmantes = DataTableFirmantes(listaResponsablesSelladores);

                        //Registrar solicitud, cambiar estatus documentos y actualizar firmante.
                        banderaFinalizarSello = new TitSolicitudDAL().ActualizarSolicitud(solicitud, dataTableDocumentos, dataTableFirmantes, dataTableBitacora);
                    }
                    else
                    {
                        //Crea una nueva solicitud.
                        titSolicitud solicitudNueva = new titSolicitud
                        {
                            solId = Guid.NewGuid().ToString(),
                            solFechaSelladoConcentradora = DateTime.Now,
                            estSolicitudId = 2, //La solicitud pasa a estatus sellado por concentradora.
                            conId = criterios.concentradora
                        };

                        listDocumentoSolicitud.Add(DocumentosPorSolicitud(solicitudNueva, listaDocumentosSellados));

                        //Se crean los Datatable.
                        DataTable dataTableBitacora = DataTableBitacoraSolicitudDocumento(listDocumentoSolicitud, pUsuario, "selloConcentradora", idConcentradora, "");
                        DataTable dataTableDocumentos = DataTableDocumentos(listDocumentoSolicitud);
                        DataTable dataTableFirmantes = DataTableFirmantes(listaResponsablesSelladores);

                        //Registrar solicitud, cambiar estatus documentos y actualizar firmante.
                        banderaFinalizarSello = new TitSolicitudDAL().RegistrarSolicitud(listDocumentoSolicitud.Select(ds => ds.solicitudDoc).ToList(), dataTableDocumentos, dataTableFirmantes, dataTableBitacora);
                    }

                    if (banderaFinalizarSello)
                    {
                        mensajeArray[0] = "true";
                        mensajeArray[1] = (documentosSellados > 1) ? "Se han firmado la cantidad de " + documentosSellados.ToString() + " documentos." : "Se ha firmado 1 documento.";

                        foreach (var solicitud in listDocumentoSolicitud)
                        {
                            Thread thread = new Thread(() => ProcesarSolicitud(solicitud.solicitudDoc.solId, perfilActivo, pUsuario));
                            thread.Start();
                        }
                    }
                    else
                    {
                        listBitacora.Add(new BitacoraML()
                        {
                            bitId = Guid.NewGuid().ToString(),
                            bitFecha = DateTime.Now,
                            bitUsuario = pUsuario,
                            bitDescripcion = "Error del sistema.",
                            bitExitoso = false,
                            accId = "selloConcentradora"
                        });

                        DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                        StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                        mensajeArray[1] = "Ha ocurrido un error, por favor intente nuevamente.";
                    }
                }
                else
                {
                    listBitacora.Add(new BitacoraML()
                    {
                        bitId = Guid.NewGuid().ToString(),
                        bitFecha = DateTime.Now,
                        bitUsuario = pUsuario,
                        bitDescripcion = "No existen registros asociados al archivo key insertado.",
                        bitExitoso = false,
                        accId = "selloConcentradora"
                    });

                    DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                    StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                    mensajeArray[1] = "No existen registros asociados al archivo .key insertado.";
                }
            }
            else
            {
                listBitacora.Add(new BitacoraML()
                {
                    bitId = Guid.NewGuid().ToString(),
                    bitFecha = DateTime.Now,
                    bitUsuario = pUsuario,
                    bitDescripcion = "No existen registros asociados al archivo key insertado.",
                    bitExitoso = false,
                    accId = "selloConcentradora"
                });

                DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                mensajeArray[1] = "No existen registros asociados al archivo .key insertado.";
            }

            return mensajeArray;
        }

        //SELLADO POR CONCENTRADORA
        public String[] SellarDocumentosConcentradora(byte[] archivoKey, String contrasenia, criteriosBusquedaTitulosML criterios, String pUsuario, String idConcentradora, PerfilML perfilActivo)
        {
            String[] mensajeArray = new String[2];
            mensajeArray[0] = "false";
            mensajeArray[1] = "";

            //Almacena las acciones que se realizaron o intentaron ejecutar.
            List<BitacoraML> listBitacora = new List<BitacoraML>();
            BitacoraML bitacora = new BitacoraML();

            //Se obtienen los documentos buscados por perfil seleccionado. 
            List<titDocumento> listaDocumentosConcentradora = new TitDocumentoDAL().ObtenerListaDocumentosFiltros(criterios);
            List<titFirmaResponsable> listTitFirmaResponsables = new TitFirmaResponsableDAL().ObtenerFirmantesByDocId(listaDocumentosConcentradora.Select(d => d.docId).ToList());
            listaDocumentosConcentradora.ForEach(d => d.titFirmaResponsable = (from firma in listTitFirmaResponsables where firma.docId == d.docId select firma).ToList());

            if (listaDocumentosConcentradora.Count != 0)
            {
                //Listas para almacenar los registros del sellado.
                List<titFirmaResponsable> listaResponsablesSelladores = new List<titFirmaResponsable>();
                List<titDocumento> listaDocumentosSellados = new List<titDocumento>();

                foreach (titDocumento documento in listaDocumentosConcentradora)
                {
                    //Obtiene el firmante del documento.
                    titFirmaResponsable firmante = new titFirmaResponsable();
                    firmante = documento.titFirmaResponsable.Where(fir => fir.firConcentradora == true).FirstOrDefault();

                    if (firmante != null)
                    {
                        try
                        {
                            //Generacion del la cadena original y sellado.
                            TituloElectronico tituloElectronico = LlenarObjetoTituloElectronico(documento);

                            //Certificado para validar.
                            byte[] bytesCertificado = Encoding.UTF8.GetBytes(firmante.firCertificadoResponsable);
                            X509Certificate2 certificado = new X509Certificate2(bytesCertificado);

                            //Complementar datos del regitro de la tabla titFirmaResponsable.
                            firmante.firCurp = ObtenerCurpCertificado(certificado);

                            String cadenaOriginal = GenerarCadenaOriginal(tituloElectronico, firmante);
                            String selloCadenaOriginal = GenerarSello(cadenaOriginal, archivoKey, contrasenia);

                            //Validacion del archivo key con el archivo cer.
                            bool banderaValidacion = ValidarSellado(cadenaOriginal, selloCadenaOriginal, certificado);

                            //En caso de ser valido el .key con .cer
                            if (banderaValidacion)
                            {
                                firmante.firSello = selloCadenaOriginal;
                                firmante.firNoCertificadoResponsable = certificado.SerialNumber;
                                firmante.firCadenaOriginal = cadenaOriginal;

                                //El documento pasa a estar en estatus sellado por concentradora.
                                documento.estDocumentoId = 3;

                                //Agregar a las listas los documentos sellados y sus firmantes.
                                listaDocumentosSellados.Add(documento);
                                listaResponsablesSelladores.Add(firmante);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }

                //Guardar registros si existieron documentos sellados.
                if (listaResponsablesSelladores.Count > 0)
                {
                    List<DocumentoSolicitud> listDocumentoSolicitud = new List<DocumentoSolicitud>();

                    //Determinacion de cantidad de solicitudes con sus bloques.
                    int maxDocumentos = 5000;
                    int totalDocumentos = listaDocumentosSellados.Count;
                    int totalSolicitudes = (totalDocumentos > maxDocumentos) ?
                        ((totalDocumentos % maxDocumentos) > 0 ? (totalDocumentos / maxDocumentos) + 1 : (totalDocumentos / maxDocumentos)) : 1;
                    int tamanioBloque = (totalDocumentos / totalSolicitudes);
                    DateTime fechaActual = DateTime.Now;

                    //Agrega los los bloques de documentos a sus respectivas solicitudes.
                    for (int i = 0, j = 0, tamBloqueFinal = 0; i <= totalSolicitudes; i++, j += tamBloqueFinal, totalSolicitudes--)
                    {
                        //Crear la nueva solicitud.
                        titSolicitud nuevaSolicitud = new titSolicitud
                        {
                            solId = Guid.NewGuid().ToString(),
                            solFechaSelladoConcentradora = fechaActual,
                            estSolicitudId = 2, //Solicitud sellada por concentradora.
                            conId = criterios.concentradora
                        };

                        //Se define el tamaño del bloque.
                        tamBloqueFinal = tamanioBloque + (listaDocumentosSellados.Skip(j).Take(totalDocumentos).Count() % totalSolicitudes > 0 ? 1 : 0);
                        List<titDocumento> bloqueDocumentos = listaDocumentosSellados.Skip(j).Take(tamBloqueFinal).ToList();

                        listDocumentoSolicitud.Add(DocumentosPorSolicitud(nuevaSolicitud, bloqueDocumentos));
                    }

                    //Se crean los DataTable
                    DataTable dataTableBitacora = DataTableBitacoraSolicitudDocumento(listDocumentoSolicitud, pUsuario, "selloConcentradora", idConcentradora, "");
                    DataTable dataTableDocumentos = DataTableDocumentos(listDocumentoSolicitud);
                    DataTable dataTableFirmantes = DataTableFirmantes(listaResponsablesSelladores);

                    //Registrar solicitud, cambiar estatus documentos y actualizar firmante.
                    bool banderaFinalizarSello = new TitSolicitudDAL().RegistrarSolicitud(listDocumentoSolicitud.Select(ds => ds.solicitudDoc).ToList(), dataTableDocumentos, dataTableFirmantes, dataTableBitacora);

                    if (banderaFinalizarSello)
                    {
                        mensajeArray[0] = "true";
                        mensajeArray[1] = (totalDocumentos > 1) ? "Se han firmado la cantidad de " + totalDocumentos.ToString() + " documentos." : "Se ha firmado 1 documento.";

                        foreach (var solicitud in listDocumentoSolicitud)
                        {
                            Thread thread = new Thread(() => ProcesarSolicitud(solicitud.solicitudDoc.solId, perfilActivo, pUsuario));
                            thread.Start();
                        }
                    }
                    else
                    {
                        listBitacora.Add(new BitacoraML()
                        {
                            bitId = Guid.NewGuid().ToString(),
                            bitFecha = DateTime.Now,
                            bitUsuario = pUsuario,
                            bitDescripcion = "Error del sistema.",
                            bitExitoso = false,
                            accId = "selloInstitucional"
                        });

                        DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                        StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                        mensajeArray[1] = "Ha ocurrido un error, por favor intente nuevamente.";
                    }
                }
                else
                {
                    listBitacora.Add(new BitacoraML()
                    {
                        bitId = Guid.NewGuid().ToString(),
                        bitFecha = DateTime.Now,
                        bitUsuario = pUsuario,
                        bitDescripcion = "No existen registros asociados al archivo key insertado.",
                        bitExitoso = false,
                        accId = "selloInstitucional"
                    });

                    DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                    StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                    mensajeArray[1] = "No existen registros asociados al archivo .key insertado.";
                }
            }
            else
            {
                listBitacora.Add(new BitacoraML()
                {
                    bitId = Guid.NewGuid().ToString(),
                    bitFecha = DateTime.Now,
                    bitUsuario = pUsuario,
                    bitDescripcion = "No existen registros asociados al archivo key insertado.",
                    bitExitoso = false,
                    accId = "selloInstitucional"
                });

                DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
                StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                mensajeArray[1] = "No existen registros asociados al archivo key insertado.";
            }

            return mensajeArray;
        }
        #endregion

        #region Sección que contiene los métodos que permiten preparar la información que se envía a SEP...

        private static TituloElectronico LlenarObjetoTituloElectronico(titDocumento documento)
        {
            TituloElectronico tituloElectronico = new TituloElectronico();
            try
            {
                foreach (var firmante in documento.titFirmaResponsable)
                {
                    tituloElectronico.folioControl = documento.docFolio;

                    #region Institucion 
                    tituloElectronico.Institucion = new TituloElectronicoInstitucion
                    {
                        cveInstitucion = documento.docInstitucionClave,
                        nombreInstitucion = documento.docInstitucionNombre

                    };
                    #endregion

                    #region Carrera
                    TituloElectronicoCarrera tituloElectronicoCarrera = new TituloElectronicoCarrera();
                    tituloElectronicoCarrera.cveCarrera = documento.docCarreraClave;
                    tituloElectronicoCarrera.nombreCarrera = documento.docCarreraNombre;
                    tituloElectronicoCarrera.fechaInicioSpecified = documento.docCarreraFechaInicio == null ? false : true;

                    if (tituloElectronicoCarrera.fechaInicioSpecified)
                    {
                        tituloElectronicoCarrera.fechaInicio = (DateTime)documento.docCarreraFechaInicio;
                    }

                    tituloElectronicoCarrera.fechaTerminacion = documento.docCarreraFechaTerminacion;
                    tituloElectronicoCarrera.idAutorizacionReconocimiento = documento.docCarreraIdAutorizacionReconocimiento;
                    tituloElectronicoCarrera.autorizacionReconocimiento = documento.docCarreraAutorizacionReconocimiento;
                    tituloElectronicoCarrera.numeroRvoe = documento.docCarreraRVOE;
                    tituloElectronico.Carrera = tituloElectronicoCarrera;
                    //tituloElectronico.Carrera = new TituloElectronicoCarrera
                    //{
                    //    cveCarrera = documento.docCarreraClave,
                    //    nombreCarrera = documento.docCarreraNombre,
                    //    fechaInicioSpecified = documento.docCarreraFechaInicio == null ? false : true,
                    //    fechaInicio = (fechaInicioSpecified == true) ? (DateTime)documento.docCarreraFechaInicio : null,
                    //    fechaTerminacion = documento.docCarreraFechaTerminacion,
                    //    idAutorizacionReconocimiento = documento.docCarreraIdAutorizacionReconocimiento,
                    //    autorizacionReconocimiento = documento.docCarreraAutorizacionReconocimiento,
                    //    numeroRvoe = documento.docCarreraRVOE,

                    //};
                    #endregion

                    #region Profesionista
                    tituloElectronico.Profesionista = new TituloElectronicoProfesionista
                    {
                        curp = documento.docProfecionistaCurp,
                        nombre = documento.docProfesionistaNombre,
                        primerApellido = documento.docProfesionistaPrimerApellido,
                        segundoApellido = documento.docProfesionistaSegundoApellido,
                        correoElectronico = documento.docProfesionistaCorreoElectronico
                    };
                    #endregion

                    #region Expedición

                    TituloElectronicoExpedicion tituloElectronicoExpedicion = new TituloElectronicoExpedicion();
                    tituloElectronicoExpedicion.fechaExpedicion = documento.docExpedicionFechaExpedicion;
                    tituloElectronicoExpedicion.idModalidadTitulacion = documento.docExpedicionIdModalidadTitulacion;
                    tituloElectronicoExpedicion.modalidadTitulacion = documento.docExpedicionModalidadTitulacion;

                    tituloElectronicoExpedicion.fechaExamenProfesionalSpecified = documento.docExpedicionFechaExamenProfesional == null ? false : true;

                    if (tituloElectronicoExpedicion.fechaExamenProfesionalSpecified)
                    {
                        tituloElectronicoExpedicion.fechaExamenProfesional = (DateTime)documento.docExpedicionFechaExamenProfesional;
                    }

                    tituloElectronicoExpedicion.fechaExencionExamenProfesionalSpecified = documento.docExpedicionFechaExencionExamenProfesional == null ? false : true;

                    if (tituloElectronicoExpedicion.fechaExencionExamenProfesionalSpecified)
                    {
                        tituloElectronicoExpedicion.fechaExencionExamenProfesional = (DateTime)documento.docExpedicionFechaExencionExamenProfesional;
                    }
                    tituloElectronicoExpedicion.cumplioServicioSocial = documento.docExpedicionCumplioServicioSocial.ToString();
                    tituloElectronicoExpedicion.idFundamentoLegalServicioSocial = documento.docExpedicionIdFundamentoLegalServicioSocial;
                    tituloElectronicoExpedicion.fundamentoLegalServicioSocial = documento.docExpedicionFundamentoLegalServicioSocial;
                    tituloElectronicoExpedicion.idEntidadFederativa = documento.docExpedicionIdEntidadFederativa;
                    tituloElectronicoExpedicion.entidadFederativa = documento.docExpedicionEntidadFederativa;

                    tituloElectronico.Expedicion = tituloElectronicoExpedicion;
                    //tituloElectronico.Expedicion = new TituloElectronicoExpedicion
                    //{
                    //    fechaExpedicion = documento.docExpedicionFechaExpedicion,
                    //    idModalidadTitulacion = documento.docExpedicionIdModalidadTitulacion,
                    //    modalidadTitulacion = documento.docExpedicionModalidadTitulacion,

                    //    fechaExamenProfesionalSpecified = documento.docExpedicionFechaExamenProfesional != Convert.ToDateTime("1800-01-01"),//definir las reglas
                    //    fechaExamenProfesional = (DateTime)documento.docExpedicionFechaExamenProfesional,
                    //    fechaExencionExamenProfesionalSpecified = documento.docExpedicionFechaExencionExamenProfesional != Convert.ToDateTime("1800-01-01"),
                    //    fechaExencionExamenProfesional = (DateTime)documento.docExpedicionFechaExencionExamenProfesional,
                    //    //cumplioServicioSocial = (documento.docExpedicionCumplioServicioSocial == 0) ? "No" : "Si",
                    //    idFundamentoLegalServicioSocial = documento.docExpedicionIdFundamentoLegalServicioSocial,
                    //    fundamentoLegalServicioSocial = documento.docExpedicionIdFundamentoLegalServicioSocial,
                    //    idEntidadFederativa = documento.docExpedicionIdEntidadFederativa,
                    //    entidadFederativa = documento.docExpedicionEntidadFederativa
                    //};
                    #endregion

                    #region Antecedente
                    TituloElectronicoAntecedente tituloElectronicoAntecedente = new TituloElectronicoAntecedente();

                    tituloElectronicoAntecedente.institucionProcedencia = documento.docAntecedenteInstitucionProcedencia;
                    tituloElectronicoAntecedente.idTipoEstudioAntecedente = documento.docAntecedenteIdTipoEstudioAntecedente;
                    tituloElectronicoAntecedente.tipoEstudioAntecedente = documento.docAntecedenteTipoEstudioAntecedente;
                    tituloElectronicoAntecedente.idEntidadFederativa = documento.docAntecedenteIdEntidadFederativa;
                    tituloElectronicoAntecedente.entidadFederativa = documento.docAntecedenteEntidadFederativa;
                    tituloElectronicoAntecedente.fechaInicioSpecified = documento.docAntecedenteFechaInicio == null ? false : true;
                    if (tituloElectronicoAntecedente.fechaInicioSpecified)
                    {
                        tituloElectronicoAntecedente.fechaInicio = (DateTime)documento.docAntecedenteFechaInicio;
                    }
                    tituloElectronicoAntecedente.fechaTerminacion = documento.docAntecedenteFechaTerminacion;
                    tituloElectronicoAntecedente.noCedula = documento.docAntecedenteNoCedula;

                    tituloElectronico.Antecedente = tituloElectronicoAntecedente;
                    //tituloElectronico.Antecedente = new TituloElectronicoAntecedente
                    //{
                    //    institucionProcedencia = documento.docAntecedenteInstitucionProcedencia,
                    //    idTipoEstudioAntecedente = documento.docAntecedenteIdTipoEstudioAntecedente,
                    //    tipoEstudioAntecedente = documento.docAntecedenteTipoEstudioAntecedente,
                    //    idEntidadFederativa = documento.docAntecedenteIdEntidadFederativa,
                    //    entidadFederativa = documento.docAntecedenteEntidadFederativa,
                    //    fechaInicioSpecified = documento.docAntecedenteFechaInicio != Convert.ToDateTime("1800-01-01"),
                    //    fechaInicio = (DateTime)documento.docAntecedenteFechaInicio,
                    //    fechaTerminacion = documento.docAntecedenteFechaTerminacion,
                    //    noCedula = documento.docAntecedenteNoCedula
                    //};
                    #endregion

                    #region FirmaResponsables
                    tituloElectronico.FirmaResponsables = (from c in documento.titFirmaResponsable
                                                           select new TituloElectronicoFirmaResponsable
                                                           {
                                                               nombre = c.firNombre,
                                                               primerApellido = c.firPrimerApellido,
                                                               segundoApellido = c.firSegundoApellido,
                                                               curp = c.firCurp,
                                                               idCargo = c.firIdCargo,
                                                               cargo = c.firCargo,
                                                               abrTitulo = c.firAbrTitulo,
                                                               sello = c.firSello,
                                                               certificadoResponsable = c.firCertificadoResponsable,
                                                               noCertificadoResponsable = c.firNoCertificadoResponsable
                                                           }).ToArray();

                    #endregion

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return tituloElectronico;
        }

        public static string GenerarCadenaOriginal(TituloElectronico titulo, titFirmaResponsable firmante)
        {
            /*
            *La cadena original se debe de generar en base a los siguientes datos
            * ||version|folioControl|firmanteCurp|firmanteIdCargo|firmanteCargo|firmanteAbrTitulo(Opcional)|institucionId|InstiticionNombre|CveCarrera|nombreCarrera|fechaInicio(Opcional)|fechaTerminacion|idAutorizacionReconocimiento|autorizacionReconocimiento|numeroRvoe|profesionistaCurp|profesionistaNombre|profesionitaPrimerApellido|profesionistaSegundoApellido(Opcional)|ProfesionistaCorreoElectronico|fechaExpedicion|idModalidadTitulacion|modalidadTitulacion|fechaExamenProfesional(Opcional)|fechaExencionExamenProfesional(opcional)|cumplioServicioSocial|idFundamentoLegalServicioSocial|fundamentoLegalServicioSocial|idEntidadFederativa|entidadFederativa|institucionProcedencia|idTipoEstudioAntecedente|tipoEstudioAntecedente |idEntidadFederativa |entidadFederativa |fechaInicio (Opcional)|fechaTerminacion |noCedula(opcional)||
            */
            try
            {
                string cadenaOriginal = "||";
                cadenaOriginal += titulo.version + "|" + titulo.folioControl + "|";

                #region Firmante
                cadenaOriginal += firmante.firCurp + "|" + firmante.firIdCargo + "|" + firmante.firCargo + "|" + firmante.firAbrTitulo + "|" + titulo.Institucion.cveInstitucion + "|" + titulo.Institucion.nombreInstitucion + "|";
                #endregion



                #region Carrera
                cadenaOriginal += titulo.Carrera.cveCarrera + "|" + titulo.Carrera.nombreCarrera + "|";


                if (titulo.Carrera.fechaInicioSpecified)
                {
                    cadenaOriginal += titulo.Carrera.fechaInicio.ToString("yyyy-MM-dd") + "|";
                }
                else
                {
                    cadenaOriginal += "|";
                }


                cadenaOriginal += titulo.Carrera.fechaTerminacion.ToString("yyyy-MM-dd") + "|" + titulo.Carrera.idAutorizacionReconocimiento + "|" + titulo.Carrera.autorizacionReconocimiento + "|" + titulo.Carrera.numeroRvoe + "|";
                #endregion

                #region Profesionista
                cadenaOriginal += titulo.Profesionista.curp + "|" + titulo.Profesionista.nombre + "|" + titulo.Profesionista.primerApellido + "|" + titulo.Profesionista.segundoApellido + "|" + titulo.Profesionista.correoElectronico + "|";
                #endregion

                #region Expedición
                if (titulo.Expedicion != null)//Se agrega if de valicación en objeto Expedición
                {
                    cadenaOriginal += titulo.Expedicion.fechaExpedicion.ToString("yyyy-MM-dd") + "|" + titulo.Expedicion.idModalidadTitulacion + "|" + titulo.Expedicion.modalidadTitulacion + "|";



                    if (!titulo.Expedicion.fechaExamenProfesionalSpecified)
                    { cadenaOriginal += "|"; }
                    else
                    { cadenaOriginal += titulo.Expedicion.fechaExamenProfesional.ToString("yyyy-MM-dd") + "|"; }

                    if (!titulo.Expedicion.fechaExencionExamenProfesionalSpecified)
                    {
                        cadenaOriginal += "|";
                    }
                    else
                    {
                        cadenaOriginal += titulo.Expedicion.fechaExencionExamenProfesional.ToString("yyyy-MM-dd") + "|";
                    }

                    cadenaOriginal += titulo.Expedicion.cumplioServicioSocial + "|" + titulo.Expedicion.idFundamentoLegalServicioSocial + "|" + titulo.Expedicion.fundamentoLegalServicioSocial + "|" + titulo.Expedicion.idEntidadFederativa + "|" + titulo.Expedicion.entidadFederativa + "|";
                }
                #endregion

                #region Antededente
                cadenaOriginal += titulo.Antecedente.institucionProcedencia + "|" + titulo.Antecedente.idTipoEstudioAntecedente + "|" + titulo.Antecedente.tipoEstudioAntecedente + "|" + titulo.Antecedente.idEntidadFederativa + "|" + titulo.Antecedente.entidadFederativa + "|";

                if (!titulo.Antecedente.fechaInicioSpecified)
                { cadenaOriginal += "|"; }
                else
                { cadenaOriginal += titulo.Antecedente.fechaInicio.ToString("yyyy-MM-dd") + "|"; }

                cadenaOriginal += titulo.Antecedente.fechaTerminacion.ToString("yyyy-MM-dd") + "|" + titulo.Antecedente.noCedula + "||";
                #endregion

                return cadenaOriginal;
            }
            catch (Exception ex) { Console.Write(ex); return null; }

        }

        public static string GenerarSello(string cadenaOriginal, byte[] fileKey, string password)
        {
            string strLlavePwd = password;
            System.Security.SecureString passwordSeguro = new System.Security.SecureString();
            passwordSeguro.Clear();

            foreach (char c in strLlavePwd.ToCharArray())
                passwordSeguro.AppendChar(c);

            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            RSACryptoServiceProvider rsa = OpenSSLKey.DecodeEncryptedPrivateKeyInfo(fileKey, passwordSeguro);
            byte[] bytesFirmados = rsa.SignData(System.Text.Encoding.UTF8.GetBytes(cadenaOriginal), hasher);

            return Convert.ToBase64String(bytesFirmados);
        }

        #endregion

        #region Generico
        public TituloML getListadoTitulos(criteriosBusquedaTitulosML filtros, int pagina, int bloque)
        {
            TitDocumentoDAL documentoDAL = new TitDocumentoDAL();
            TituloML titulo = new TituloML();
            titulo.listadoTitulo = documentoDAL.ObtenerListaDocumentosInstitucion(filtros, pagina, bloque);
            titulo.totalRegistros = documentoDAL.countDocumentosSolicitud(filtros);
            return titulo;
        }
        public SolicitudML getListadoSolicitudes(criteriosBusquedaSolicitudML filtros, int pagina, int bloque)
        {
            TitSolicitudDAL solicitudDAL = new TitSolicitudDAL();
            SolicitudML solicitud = new SolicitudML();
            solicitud.listViewTitSolicitud = solicitudDAL.ObtenerListaSolicitudesConcentradora(filtros, pagina, bloque);
            solicitud.totalRegistros = solicitudDAL.countSolicitudesConcentradora(filtros);
            return solicitud;
        }
        public SolicitudML getDatosSolicitud(criteriosBusquedaSolicitudML filtros, String solId)
        {
            return new TitSolicitudDAL().getDatosSolicitudById(filtros, solId);
        }
        public TituloML getDatosTitulo(criteriosBusquedaTitulosML filtros, string docId)
        {
            TituloML titulo = new TitDocumentoDAL().getDatosDocumentoById(filtros, docId);
            return titulo;
        }
        public List<titFirmaResponsable> getListadoFirmantes(string docId)
        {
            return new TitFirmaResponsableDAL().ObtenerListaFirmantesPorDocumentos(docId.Split(',').ToList());
        }

        //CONVIERTE UN ARCHIVO DE TIPO HttpPostedFileBase A UN TIPO byte[].
        public byte[] ConvertirArchivoHttpABytes(HttpPostedFileBase archivo)
        {
            byte[] data;

            using (Stream inputStream = archivo.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }

            return data;
        }

        //METODO PARA ELIMINAREL REGISTRO DE UN DOCUMENTO
        public bool EliminarDocumento(string pDocId) => new TitDocumentoDAL().EliminarDocumento(pDocId);

        //METODO PARA VALIDAR EL SELLADO DEL ARCHIVO .KEY CONTRA EL ARCHIVO .CER.
        public bool ValidarSellado(String cadenaOriginal, String selloCadenaOriginal, X509Certificate2 certificado)
        {
            bool banderaValidacion = false;
            byte[] bytesCadenaOriginal = Encoding.UTF8.GetBytes(cadenaOriginal);
            byte[] bytesSellados = Convert.FromBase64String(selloCadenaOriginal);

            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            RSACryptoServiceProvider RSA = (RSACryptoServiceProvider)certificado.PublicKey.Key;
            banderaValidacion = RSA.VerifyData(bytesCadenaOriginal, hasher, bytesSellados);

            return banderaValidacion;
        }

        //METODO PARA OBTENER EL CURP DEL CERTIFICADO.
        public String ObtenerCurpCertificado(X509Certificate2 certificado)
        {
            String[] datosFirmante = certificado.SubjectName.Name.Split(',');
            String[] claveValor = datosFirmante[0].Split('=');
            String curpFirmante = claveValor[1];

            return curpFirmante;
        }

        //Valida la contraseña de un archivo .key.
        public bool ValidaKeyContraseña(byte[] archivoKey, String contrasenia)
        {
            bool banderaValidacion = false;

            try
            {
                String mensajeValidar = "Mensaje de validación para archivo .key con su contraseña.";
                GenerarSello(mensajeValidar, archivoKey, contrasenia);
                banderaValidacion = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return banderaValidacion;
        }

        //Obtiene un registro de configuracion de concentradora por su Id.
      
        //public titConfiguracionConcentradora ObtenerRegistroConfiguracionPorId(String IdConcentradora) =>
        //new TitConfiguracionConcentradoraDAL().ObtenerRegistroConfiguracionPorId(IdConcentradora);


        //Retorna una modelo que contiene una solicitud y su lista de documentos.
        public DocumentoSolicitud DocumentosPorSolicitud(titSolicitud solicitud, List<titDocumento> listDocumentos)
        {
            DocumentoSolicitud oDocumentoSolicitud = new DocumentoSolicitud();
            oDocumentoSolicitud.listaDocumentoSol.AddRange(listDocumentos);
            oDocumentoSolicitud.solicitudDoc = solicitud;

            return oDocumentoSolicitud;
        }

        //Retorna una bandera dependiendo si la concentradora actua como institucion o no, true = institucion, false = concentradora
        public bool ConcentradoraComoInstitucion(PerfilML perfilActivo)
        {
            if (perfilActivo != null)
            {
                String idConcentrdora = (perfilActivo.accesos.Select(t => t.concentradora).ToList() == null) ? "" : perfilActivo.accesos.Select(t => t.concentradora).ToList().Distinct().FirstOrDefault();
                return new ConfiguracionConcentradoraBL().ObtenerConfiguracionCifradaPorId(idConcentrdora).conFirmaInsitucion ?? false;
            }
            else
            {
                return false;
            }
        }

        public String getCarrerasByInstitucionClave(string institucionId, PerfilML perfil)
        {
            string concentradora = (perfil.accesos.Select(t => t.concentradora).ToList() == null) ? "" : perfil.accesos.Select(t => t.concentradora).ToList().Distinct().FirstOrDefault();
            var listCarreras = perfil.accesos.Where(t => t.institucion == institucionId).Select(t => t.carrera).Distinct().Where(x => x != "").ToList();
            listCarreras = listCarreras.Count == 0 ? new TitDocumentoDAL().getListadoCarreras(new List<String> { institucionId }, concentradora) : listCarreras;

            string comboCarreras = listCarreras.Count == 0 ? "<option value=''>Todos</option>" : "";
            foreach (var item in listCarreras)
            {
                comboCarreras += "<option value='" + item + "'>" + item + "</option>";
            }
            return comboCarreras;
        }

        // Generar pdf del título, regresa un stream del documento
        public Stream GenerarPDF(string docId)
        {

            // Obtener titDocumento y el id de plantilla que le corresponde
            titDocumento documento = new TitDocumentoDAL().getDatostitDocumentoById(docId);
            string planId = documento.planId;

            // Obtener nombre de plantilla de la bd y ruta de la carpeta content para verificar existencia del archivo

           
            string planNombre = documento.planId + ".docx";
            string rutaCarpetaContent = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"]);
            string rutaPlantilla = Path.Combine(rutaCarpetaContent, documento.planId, planNombre);
            if (!Directory.Exists(rutaCarpetaContent)) Directory.CreateDirectory(rutaCarpetaContent);
            if (!File.Exists(rutaPlantilla))
            {


                titCatPlantilla plantilla = ObtenertitCatPlantillaById(planId);


              
                PlantillaBL.extraerFilePlantilla(plantilla.planArchivo,plantilla.planId);

                 // File.WriteAllBytes(rutaPlantilla,plantilla.planArchivo);
            }


            bool tituloGenerado = false;

                // Obtenemos ruta de la carpeta Content/Plantillas del proyecto para verificar la existencia de la plantilla
                
                string xml = documento.docXML;

                List<KeyValuePair<string, string>> adicionales = new List<KeyValuePair<string, string>>();

                if (String.IsNullOrWhiteSpace(documento.docEtiquetaDescripcion))
                {
                    documento.docEtiquetaDescripcion = "Título";
                }

                adicionales.Add(new KeyValuePair<string, string>("etiquetaDescripcion", documento.docEtiquetaDescripcion));

                XDocument xmldoc = new XDocument();
                if (!string.IsNullOrEmpty(xml))
                {
                    using (StringReader str = new StringReader(xml))
                    {
                        tituloGenerado = true;
                        xmldoc = XDocument.Load(str);

                        string rutaQr = ConfigurationManager.AppSettings["urlQrDocumento"] + docId;
                        return PDF.GeneratePDFfromXML(rutaPlantilla, xmldoc, rutaQr, adicionales);
                    }
                }
            

            // Si no se generó el título mandamos pdf con mensaje
            if (!tituloGenerado)
            {
                string rutaTituloNoDisponible = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaTituloNoDisponible"]);
                return new FileStream(rutaTituloNoDisponible, FileMode.Open);
            }

            return null;
        }

        public bool EnvioCorreo(string sDestinatario, string CC, string Asunto, string Cuerpo, string Ruta)
        {

            ServiciosWeb.EnvioCorreo.WSCorreoSoapClient wSCorreo = new ServiciosWeb.EnvioCorreo.WSCorreoSoapClient();

            try
            {
                var ewSCorreo = wSCorreo.Enviarcorreo(sDestinatario, CC, Asunto, Cuerpo, Ruta);
                return ewSCorreo[0].claveEstatus == 0 ? true : false;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }
        public Boolean guardaObservacionesTitulo(TituloML titulo, string usrLogin, string accId)
        {
            titObservacion observacion = new titObservacion
            {
                obsId = Guid.NewGuid().ToString(),
                obsDescripcion = titulo.observaciones,
                obsFecha = DateTime.Now,
                obsUsuario = usrLogin,
                docId = titulo.docId,
                motId = titulo.idMotivoCancelacion,
                accId = accId
            };
            return new TitObservacionDAL().guardaObservaciones(observacion);
        }


        //Retorna un DataTable de los movimientos realizados en el sellado.
        public DataTable DataTableBitacoraSolicitudDocumento(List<DocumentoSolicitud> listDocumentoSolicitud, String pUsuario, String pIdAccion, String concentradora, String institucion)
        {
            DataTable dataTableBitacora = new DataTable();
            List<BitacoraML> listBitacora = new List<BitacoraML>();
            BitacoraML bitacora = new BitacoraML();
            DateTime fechaActual = DateTime.Now;

            foreach (DocumentoSolicitud docSol in listDocumentoSolicitud)
            {
                foreach (titDocumento doc in docSol.listaDocumentoSol)
                {
                    //Crear listado de acciones por registro sellado.
                    bitacora = new BitacoraML()
                    {
                        bitId = Guid.NewGuid().ToString(),
                        bitFecha = fechaActual,
                        bitUsuario = pUsuario,
                        bitDescripcion = String.Format("conId: {0}," + ((String.IsNullOrEmpty(institucion)) ? "" : (String.Format(" docInstitucionClave: {0}", institucion))) + " solId: {1}, docId: {2}", concentradora, docSol.solicitudDoc.solId, doc.docId),
                        bitExitoso = true,
                        accId = pIdAccion
                    };

                    listBitacora.Add(bitacora);
                }
            }

            return MetodosGenericosBL.ConvertToDataTable(listBitacora);
        }

        public DataTable DataTableDocumentos(List<DocumentoSolicitud> listDocumentoSolicitud)
        {
            List<DocumentoSelladoML> listaDocumentos = new List<DocumentoSelladoML>();
            DocumentoSelladoML oDocumento = new DocumentoSelladoML();

            foreach (DocumentoSolicitud docSol in listDocumentoSolicitud)
            {
                foreach (titDocumento doc in docSol.listaDocumentoSol)
                {
                    oDocumento = new DocumentoSelladoML()
                    {
                        docId = doc.docId,
                        solId = docSol.solicitudDoc.solId,
                        estDocumentoId = doc.estDocumentoId
                    };

                    listaDocumentos.Add(oDocumento);
                }
            }

            return MetodosGenericosBL.ConvertToDataTable(listaDocumentos);
        }

        public DataTable DataTableFirmantes(List<titFirmaResponsable> listaResponsablesSelladores)
        {
            List<FirmaResponsableSelladoML> listaFirmantes = new List<FirmaResponsableSelladoML>();
            FirmaResponsableSelladoML oResponsable = new FirmaResponsableSelladoML();

            foreach (titFirmaResponsable firmante in listaResponsablesSelladores)
            {
                oResponsable = new FirmaResponsableSelladoML()
                {
                    docId = firmante.docId,
                    firCurp = firmante.firCurp,
                    firSello = firmante.firSello,
                    firNoCertificadoResponsable = firmante.firNoCertificadoResponsable,
                    firCadenaOriginal = firmante.firCadenaOriginal,
                    firConcentradora = firmante.firConcentradora
                };

                listaFirmantes.Add(oResponsable);
            }

            return MetodosGenericosBL.ConvertToDataTable(listaFirmantes);
        }


        //Obtener documento por Id
        public titDocumento getDatostitDocumentoById(String docId) => new TitDocumentoDAL().getDatostitDocumentoById(docId);


        //OBTIENE REGISTRO DE [ViewXMLtitDocumento] PARA PORTAL DE VALIDACIÓN.
        public ViewXMLtitDocumento ObtenerDocumentoViewXML(String idDocumento) => new TitDocumentoDAL().ObtenerDocumentoViewXML(idDocumento);

        public TituloML getListadoTitulosByCURP(string curp, string conId)
        {
            TituloML titulo = new TituloML();
            titulo.listadoTitulo = new TitDocumentoDAL().getDocumentosByCURP(curp, conId);
            return titulo;
        }

        #endregion


        #region Métodos para la consulta de solicitudes finalizadas
        public String[] enviaCorreoProfesionista(TituloML titulo, criteriosBusquedaTitulosML filtros, string usrLogin, bool sinObservaciones = true)
        {
            String[] result = new String[2];
            List<BitacoraML> listBitacora = new List<BitacoraML>();

            string cuerpoMensaje = crearCuerpoMensaje(titulo);
            string asunto = cuerpoMensaje == "" ? "" : "Solicitud de título electrónico";

            result[1] = (result[0] = verificarExisteTitulo(titulo, filtros).ToString()) == "True" ?

                    (result[0] = EnvioCorreo(titulo.docProfesionistaCorreoAlternativo, "", asunto, cuerpoMensaje, "").ToString()) == "True" ?

                         (sinObservaciones || actualizaCorreoTitulo(titulo)) && (sinObservaciones || guardaObservacionesTitulo(titulo, usrLogin, "enviarCorreo")) ?
                         "Se envió el correo y actualizó el registro de título." : "Se envió el correo."
                    : "No fue posible enviar el correo."
                : "No cuenta con acceso a este registro de título.";

            listBitacora.Add(new BitacoraML()
            {
                bitId = Guid.NewGuid().ToString(),
                bitFecha = DateTime.Now,
                bitUsuario = usrLogin,
                bitDescripcion = "docId: " + titulo.docId + " correo: " + titulo.docProfesionistaCorreoAlternativo + " mensaje: " + result[1],
                bitExitoso = Convert.ToBoolean(result[0]),
                accId = "enviarCorreo"
            });

            DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
            StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

            return result;
        }
        public Boolean verificarExisteTitulo(TituloML titulo, criteriosBusquedaTitulosML filtros)
        {
            bool result = false;
            TituloML tituloML = new TitDocumentoDAL().getDatosDocumentoById(filtros, titulo.docId);
            result = tituloML.documento.docId != null ? true : result;
            return result;
        }
        public Boolean actualizaCorreoTitulo(TituloML titulo)
        {
            return new TitDocumentoDAL().actualizaCorreo(titulo);
        }
        public String crearCuerpoMensaje(TituloML titulo)
        {
            string rutaPortal = ConfigurationManager.AppSettings["rutaPortalDescarga"];
            string rutaAvisoprivacidad = ConfigurationManager.AppSettings["rutaAvisoprivacidad"];
            string rutaPaginaSEG = ConfigurationManager.AppSettings["rutaPaginaSEG"];
            string codigo = obtenerToken(titulo);
            codigo = codigo == "" ? obtenerToken(titulo) : codigo;

            return codigo == "" ? "" : "<div style='font-size: 16px;font-family: Arial;'>" +
                        "<p>Para descargar el título electrónico ingresa el siguiente código en el portal de la " +
                            "Secretaría de Educación de Guanajuato, " +
                            "<a href='" + rutaPortal + "'>" + rutaPortal + "</a>" +
                        "</p>" +
                "<div>" +
                "<div style='font-size: 21.5px;font-family: Times New Roman,Arial;font-weight: bold;text-align:center'>" +
                    codigo +
                "</div>" +
                "<div style='font-size: 12px;font-family: Arial;text-align:center'>" +
                "Este es un mensaje automático y no es necesario responder." +
                "</div>" +
                "<div style='font-style: italic;font-size: 10.5px;font-family: Arial;text-align:justify'>" +
                    "<p>Aviso de Privacidad</p>" +
                    "<p>Versión Simplificada</p>" +
                    "<p>La Secretaría de Educación del Estado de Guanajuato (en adelante Secretaría), de conformidad con lo establecido en los artículos:" +
                        " 6, apartado A, fracción II, y 16 párrafo segundo de la Constitución Política para los Estados Unidos Mexicanos; 14 inciso B, fracciones" +
                        " II y III, de la Constitución Política para el Estado de Guanajuato; 13 fracción III, 15, 25, de la Ley Orgánica del Poder Ejecutivo para el" +
                        " Estado de Guanajuato y 13, 35, 36, 37, 38, 96, 97, 98, 99, 100 y 101 de la Ley de Protección de Datos Personales en Posesión de Sujetos " +
                        "Obligados para el Estado de Guanajuato, le informa que la protección de sus datos personales es un derecho humano vinculado a la protección" +
                        " de su privacidad.<br>Cabe señalar que los datos personales, se refieren a cualquier información concerniente a una persona física identificada " +
                        "o identificable y los datos personales sensibles, son aquellos que afecten a la esfera más íntima de su titular, o cuya utilización indebida" +
                        " pueda dar origen a discriminación o conlleve un riesgo grave para éste.</p>" +
                    "<p>Sus datos personales de conformidad con las funciones propias de esta Secretaría pueden ser utilizados para las siguientes finalidades:<br>" +
                        "*Para la prestación del servicio educativo: Procesos de inscripciones, revalidaciones, certificaciones, autenticaciones, titulaciones, " +
                        "tramitación y expedición de cédulas profesionales, incorporaciones de instituciones particulares para impartir educación, así como diversos apoyos a" +
                        " través de programas.<br>*Para la administración de Recursos Humanos y Servicio Profesional: Contratación de personal administrativo (base o confianza)," +
                        " personal de apoyo a la educación, personal docente a través de las convocatorias para el ingreso, promoción, reconocimiento y permanencia en el" +
                        " Servicio Profesional Docente, temas relacionados con la Seguridad Social de los Trabajadores de esta Secretaría; diversos apoyos a través de programas, " +
                        "contratación de servicios profesionales.</p>" +
                    "<p>El Aviso de Privacidad Integral puede ser consultado en la página Institucional de esta Secretaría (<a href='" + rutaPaginaSEG + "'>" + rutaPaginaSEG + "</a>) o bien, de manera" +
                        " directa en la siguiente liga electrónica: <a href='" + rutaAvisoprivacidad + "'>" + rutaAvisoprivacidad + "</a> </p>" +
                "</div>";
        }
        public String obtenerToken(TituloML titulo)
        {
            string codigo = generaCodigo();
            titTokenDescarga token = new titTokenDescarga
            {
                tokCodigo = codigo,
                docId = titulo.docId,
                tokfechaCreacion = DateTime.Now,
                tokfechaDescarga = null,
                tokEstatus = true,
                tokUsado = false,
            };
            return new TitTokenDescargaDAL().guardaTokenDescarga(token) ? codigo : "";
        }
        public String generaCodigo()
        {
            var chars = Guid.NewGuid().ToString().Replace("-", "") + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[10];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);

            return finalString;
        }
        public TituloML getDatosCancelaTitulo(criteriosBusquedaTitulosML filtros, string idDocumento)
        {
            TituloML tituloML = new TituloML();

            tituloML = getDatosTitulo(filtros, idDocumento);
            tituloML.comboMotivosCancelacion = from c in new TitCatMotivosCancelacionDAL().getMotivosCancelacion() select new SelectListItem { Value = c.motId.ToString(), Text = c.motDescripcion.ToString() };

            return tituloML;
        }
        public String[] cancelaRegistroDeTitulo(TituloML titulo, criteriosBusquedaTitulosML filtros, string usrLogin)
        {
            String[] result = new String[2];
            TitDocumentoDAL titDocumento = new TitDocumentoDAL();
            List<BitacoraML> listBitacora = new List<BitacoraML>();

            result[1] = (result[0] = verificarExisteTitulo(titulo, filtros).ToString()) == "True" ?
                titDocumento.actualizaEstatus(new TituloML { docId = titulo.docId, estDocumentoId = 7 }) ?
                    (result[0] = (cancelaRegistroDeTituloEnSEP(titDocumento.getDatostitDocumentoById(titulo.docId), titulo.idMotivoCancelacion, filtros, usrLogin) && guardaObservacionesTitulo(titulo, usrLogin, "cancelarRegistro")).ToString()) == "True" ?
                        "Se canceló el registro." : titDocumento.actualizaEstatus(new TituloML { docId = titulo.docId, estDocumentoId = 8 }) ?
                                        "No fue posible cancelar el registro." : titDocumento.actualizaEstatus(new TituloML { docId = titulo.docId, estDocumentoId = 8 }).ToString()
                    : "No fue posible cancelar el registro."
                : "No fue posible cancelar el registro.";


            listBitacora.Add(new BitacoraML()
            {
                bitId = Guid.NewGuid().ToString(),
                bitFecha = DateTime.Now,
                bitUsuario = usrLogin,
                bitDescripcion = "docId: " + titulo.docId + " correo: " + titulo.docProfesionistaCorreoAlternativo + " mensaje: " + result[1],
                bitExitoso = Convert.ToBoolean(result[0]),
                accId = "cancelarRegistro"
            });

            DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
            StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

            return result;
        }

        public Boolean cancelaRegistroDeTituloEnSEP(titDocumento documento, string idMotivoCancelacion, criteriosBusquedaTitulosML filtros, string usrLogin)
        {
            titConfiguracionConcentradora titConfiguracion = new TitConfiguracionConcentradoraDAL().ObtenerRegistroConfiguracionPorId(filtros.concentradora);
           /* var cancelaTitulo =   new SEPSA().CancelaTituloElectronico(documento.docFolio, idMotivoCancelacion, titConfiguracion.conUsuarioWS, EncriptarAES.DecryptStringAES(titConfiguracion.conContrasenaWS));

            TituloML titulo = new TituloML
            {
                docId = documento.docId,
                observaciones = cancelaTitulo.mensaje,
                idMotivoCancelacion = idMotivoCancelacion
            };

            guardaObservacionesTitulo(titulo, usrLogin, "cancelarRegistro");
            */
            return /*cancelaTitulo.codigo == 0 ? true : */false;
        }
        public void gurdaBitacoraDescargaTitulo(string idDocumento, string usrLogin, string accion)
        {
            List<BitacoraML> listBitacora = new List<BitacoraML>();
            listBitacora.Add(new BitacoraML()
            {
                bitId = Guid.NewGuid().ToString(),
                bitFecha = DateTime.Now,
                bitUsuario = usrLogin,
                bitDescripcion = "docId: " + idDocumento + " mensaje: Se realizó una descarga del documento.",
                bitExitoso = true,
                accId = accion
            });

            DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
            StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");
        }
        public TituloML getDatosCambioPlantilla(criteriosBusquedaTitulosML filtros, string idDocumento)
        {
            TituloML tituloML = new TituloML();

            tituloML = getDatosTitulo(filtros, idDocumento);
            tituloML.comboPlantillas = from c in new TitCatPlantillaDAL().ViewTitCatPlantillasConcentradora(filtros.concentradora, 0, 100000000) select new SelectListItem { Selected = c.planid == tituloML.documento.planId, Value = c.planid.ToString(), Text = c.planNombre.ToString() };

            return tituloML;
        }
        public String[] cambiaPlantillaDeTitulo(TituloML titulo, string usrLogin)
        {
            string[] result = new string[2];

            result[1] = (result[0] = new TitDocumentoDAL().actualizaPlantillaByDocumentoId(titulo).ToString()) == "True" ?
                "Se cambió la plantilla." : "No fue posible actualizar la plantilla.";

            return result;
        }
        #endregion

        #region Preparar información para enviar a SEP

        public SolicitudML getListadoProcesoSEP(criteriosBusquedaSolicitudML filtros, int pagina, int bloque)
        {
            TitSolicitudDAL solicitudDAL = new TitSolicitudDAL();
            SolicitudML solicitud = new SolicitudML();
            solicitud.listViewTitSolicitud = solicitudDAL.ObtenerListaSolicitudesConcentradora(filtros, pagina, bloque);
            solicitud.totalRegistros = solicitudDAL.countSolicitudesConcentradora(filtros);
            return solicitud;
        }

        public string[] ProcesarSolicitud(string solId, PerfilML perfilActivo, string userId)
        {
            titSolicitud solicitud = new TitSolicitudDAL().ObtenerRegistroSolicitud(solId);
            int? iEstatusSolicitud = solicitud.estSolicitudId;

            string[] resultado = new string[2];

            resultado[0] = "false";
            resultado[1] = "No fue posible realizar la acción.";
            criteriosBusquedaTitulosML filtros = new criteriosBusquedaTitulosML(perfilActivo, new criteriosBusquedaTitulosML());
            filtros.estatus = "3,4,5,6";
            filtros.idSolicitud = solId;
            titConfiguracionConcentradora configuracionConcentradora = new titConfiguracionConcentradora();

            configuracionConcentradora = new TitConfiguracionConcentradoraDAL().ObtenerRegistroConfiguracionPorId(filtros.concentradora);

            List<titDocumento> documentos = new TitDocumentoDAL().ObtenerListaDocumentosFiltros(filtros);
            if (documentos.Count > 0)
            {
                solicitud.estSolicitudId = 3;//Estatus a procesando solicitud para que no se realicen peticiones sobre la misma solicitud.
                new TitSolicitudDAL().AgregarActualizarSolicitud(solicitud);

                solicitud.estSolicitudId = iEstatusSolicitud;

                if (solicitud.estSolicitudId == 2)
                {
                    EnviarLote(configuracionConcentradora, filtros, ref documentos, ref solicitud, userId);
                }

                if (solicitud.estSolicitudId == 4)
                {
                    ObtenerEstatusLote(configuracionConcentradora, filtros, ref documentos, ref solicitud);
                }

                if (solicitud.estSolicitudId == 5)
                {
                    DescargarResultadoLote(configuracionConcentradora, ref solicitud);
                }

                if (solicitud.estSolicitudId == 6)
                {
                    ActualizarRegistrosSolicitud(ref solicitud, userId);
                }

                new TitSolicitudDAL().AgregarActualizarSolicitud(solicitud);
            }
            else
            {
                resultado[1] = "No se encontraron documentos asociados a la solicitud.";
            }
            return resultado;
        }

        public Boolean EnviarLote(titConfiguracionConcentradora configuracionConcentradora, criteriosBusquedaTitulosML filtros, ref List<titDocumento> documentos, ref titSolicitud solicitud, string userId)
        {
            Boolean resultado = false;

            int numeroIntentos = 10;
            try
            {
                solicitud.estSolicitudId = 2; //Se regresa al estatus inicial para que en caso de que no se envie con exito se quede disponible para enviar nuevamente.

                List<(TituloElectronico, string)> tituloElectronicos = (from documento in documentos select (LlenarObjetoTituloElectronico(documento), documento.docId)).ToList();

                if (solicitud.solArchivoEnvioSEP == null)
                {
                    solicitud.solArchivoEnvioSEP = GenerarZip(tituloElectronicos, solicitud.solId, userId);

                }

                int intento = 0;
                while (!resultado && intento < numeroIntentos)
                {

                    var resultadoCarga = new SEPSA().EnviarCarga(solicitud.solArchivoEnvioSEP, solicitud.solId + ".zip", configuracionConcentradora.conUsuarioWS, EncriptarAES.DecryptStringAES(configuracionConcentradora.conContrasenaWS));

                    if (resultadoCarga != null && !string.IsNullOrWhiteSpace(resultadoCarga.numeroLote))
                    {
                        solicitud.solFolioLoteSEP = resultadoCarga.numeroLote;
                        solicitud.solMensajeEnvioSEP = resultadoCarga.mensaje;
                        solicitud.solFechaEnvioSEP = DateTime.Now;
                        solicitud.estSolicitudId = 4;
                        resultado = true;
                    }
                    intento++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return resultado;
        }

        public Boolean ObtenerEstatusLote(titConfiguracionConcentradora configuracionConcentradora, criteriosBusquedaTitulosML filtros, ref List<titDocumento> documentos, ref titSolicitud solicitud)
        {
            Boolean resultado = false;
            int numeroIntentos = 30;
            try
            {
                while (!resultado && numeroIntentos < 31)
                {
                    var estatusLote = new SEPSA().ConsultarEstatusCarga(solicitud.solFolioLoteSEP, configuracionConcentradora.conUsuarioWS, EncriptarAES.DecryptStringAES(configuracionConcentradora.conContrasenaWS));

                    resultado = !String.IsNullOrEmpty(estatusLote.numeroLote) ? true : false;
                    solicitud.solMensajeConsultaSEP = estatusLote.mensaje;
                    solicitud.solFechaRespuestaSEP = DateTime.Now;
                    if (resultado)
                    {
                        solicitud.estSolicitudId = 5;
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
                //Actualizar estatus de la solicitud cuando sea exitosa.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return resultado;
        }

        public Boolean DescargarResultadoLote(titConfiguracionConcentradora configuracionConcentradora, ref titSolicitud solicitud)
        {
            Boolean resultado = false;
         /*   int numeroIntentos = 0;
            try
            {
                while (!resultado && numeroIntentos < 31)
                {
                    var descarga = new SEPSA().DescargaResultado(solicitud.solFolioLoteSEP, configuracionConcentradora.conUsuarioWS, EncriptarAES.DecryptStringAES(configuracionConcentradora.conContrasenaWS));
                    solicitud.solArchivoResultadoSEP = descarga.titulosBase64;
                    solicitud.solMensajeResultadoSEP = descarga.mensaje;

                    if (descarga.titulosBase64 != null)
                    {
                        solicitud.estSolicitudId = 6;
                        resultado = true;
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                    numeroIntentos++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }*/
            return resultado;
        }

        private Boolean ActualizarRegistrosSolicitud(ref titSolicitud solicitud, string userId)
        {
            Boolean resultado = false;
            try
            {
                //Obtener Listado de la solicitud
                //Leer archivo Excel
                string rutaExcel = "";
                string sMensajeResultadoSEP = "";
                bool bBanderaActualizarRegistro = false;

                //rutaExcel = Path.Combine(ConfigurationManager.AppSettings["repositorioFileSEP"], "resultado", "_" + solicitud.solId);
                rutaExcel = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), "Temporal", "Recursos", "Resultado");
                if (!Directory.Exists(rutaExcel))
                {
                    Directory.CreateDirectory(rutaExcel);
                }
                List<ResultExcelML> datosExcel = new List<ResultExcelML>();
                File.WriteAllBytes(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), solicitud.solArchivoResultadoSEP);

                if (!Directory.Exists(Path.Combine(rutaExcel, "_" + solicitud.solId)))
                {
                    Directory.CreateDirectory(Path.Combine(rutaExcel, "_" + solicitud.solId));
                }
                else
                {
                    Directory.Delete(Path.Combine(rutaExcel, "_" + solicitud.solId), true);
                    Directory.CreateDirectory(Path.Combine(rutaExcel, "_" + solicitud.solId));
                }

                try
                {
                    ZipFile.ExtractToDirectory(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), Path.Combine(rutaExcel, "_" + solicitud.solId));
                }
                catch (Exception ex)
                {
                    //try de si existe el archivo excel
                }
                var directorioResultado = new DirectoryInfo(Path.Combine(rutaExcel, "_" + solicitud.solId));
                foreach (var file in directorioResultado.GetFiles())
                {
                    rutaExcel = Path.Combine(rutaExcel, "_" + solicitud.solId, file.Name);
                }
                try
                {
                    using (FileStream stream = File.Open(rutaExcel, FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader excelReader = rutaExcel.Contains(".xlsx") ? ExcelReaderFactory.CreateOpenXmlReader(stream) : ExcelReaderFactory.CreateBinaryReader(stream);

                        var result = excelReader.AsDataSet();
                        Boolean rowEncabezado = true;
                        ResultExcelML resultExcelML = new ResultExcelML();

                        if (excelReader.RowCount > 2)
                        {
                            bBanderaActualizarRegistro = true;
                        }
                        else
                        {
                            bBanderaActualizarRegistro = false;
                        }

                        while (excelReader.Read())
                        {
                            if (!rowEncabezado)
                            {
                                resultExcelML = new ResultExcelML();
                                resultExcelML.archivo = excelReader[0].ToString();
                               // resultExcelML.estatus = excelReader[1].ToString();
                                resultExcelML.mensaje = excelReader[2].ToString();
                                if (excelReader[3] != null)
                                {
                                    resultExcelML.folioControl = excelReader[3].ToString();
                                    bBanderaActualizarRegistro = true;
                                }
                                else
                                {
                                    sMensajeResultadoSEP = excelReader[2].ToString() + ", ";
                                }
                                resultExcelML.docId = Path.GetFileNameWithoutExtension(excelReader[0].ToString());
                                resultExcelML.estatusdocumento = excelReader[1].ToString().Equals("1") ? 5 : 6;
                                resultExcelML.dtFecha = DateTime.Now;
                                resultExcelML.observacionId = Guid.NewGuid().ToString();
                                resultExcelML.accId = "estatusSEP";
                                resultExcelML.obsUsuario = userId;
                                datosExcel.Add(resultExcelML);
                            }

                            rowEncabezado = false;
                        }
                    }
                    //Realizar el impacto en la base de datos:  considerando estatus retornado por sep y el mensaje
                    DataTable dtResultExcel = StoredProcedure.ConvertToDataTable(datosExcel);
                    StoredProcedure.Merged(dtResultExcel, "typeResultExcel", "spMergedEstatusDocumento", "typeResultExcel");

                    if (bBanderaActualizarRegistro)
                    {
                        solicitud.estSolicitudId = 7;
                    }
                    else
                    {
                        solicitud.solMensajeResultadoSEP = "Ocurrió un error en la solicitud. " + sMensajeResultadoSEP;
                        solicitud.estSolicitudId = 9;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            catch (Exception ex)
            {

            }
            return resultado;

        }

        private static Byte[] GenerarZip(List<(TituloElectronico tituloElectronico, string docId)> titulos, string idBloque, string userId)
        {


            string pathZIP = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), "Temporal", "Recursos", "Envio");
            if (!Directory.Exists(pathZIP))
            {
                Directory.CreateDirectory(pathZIP);
            }


            //string pathZIP = ConfigurationManager.AppSettings["repositorioFileSEP"] + "envio";

            List<(string, byte[])> foliosXML = new List<(string, byte[])>();
            List<TitDocumentoML> listTitDocumentoML = new List<TitDocumentoML>();
            List<BitacoraML> listTitBitacora = new List<BitacoraML>();

            Directory.CreateDirectory(pathZIP + "//" + idBloque);

            if (File.Exists(Path.Combine(pathZIP, idBloque + ".zip")))
            {
                File.Delete(Path.Combine(pathZIP, idBloque + ".zip"));
            }

            ZipFile.CreateFromDirectory(pathZIP + "//" + idBloque, pathZIP + "//" + idBloque + ".zip", System.IO.Compression.CompressionLevel.Optimal, false);
            FileStream zipToOpen = new FileStream(pathZIP + "//" + idBloque + ".zip", FileMode.Open);
            Directory.Delete(pathZIP + "//" + idBloque);

            try
            {
                var serializer = new XmlSerializer(typeof(TituloElectronico));

                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (var titulo in titulos)
                    {
                        var nameFile = titulo.docId + ".xml";
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(nameFile);

                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            using (ExtendedStringWriter stringXML = new ExtendedStringWriter(Encoding.UTF8))
                            {
                                serializer.Serialize(stringXML, titulo.tituloElectronico);
                                writer.WriteLine(stringXML.ToString());
                                writer.Flush();
                                writer.Close();
                            }
                        }

                        //Pasar el archivo a byte[] para que se guarde en la base de datos en el campo xmlEnvioSEP
                        Byte[] documento;
                        using (MemoryStream file = new MemoryStream())
                        {
                            var archivo = readmeEntry.Open();
                            archivo.CopyTo(file);
                            documento = file.ToArray();

                            listTitDocumentoML.Add(new TitDocumentoML() { docId = titulo.docId, sXML = documento });
                            listTitBitacora.Add(new BitacoraML() { bitId = Guid.NewGuid().ToString(), accId = "envioSEP", bitExitoso = true, bitDescripcion = "Envio Sep correcto docId: " + titulo.docId + "", bitFecha = DateTime.Now, bitUsuario = userId });

                            foliosXML.Add((idBloque, documento));
                        }
                    }
                }

                DataTable dtTitDocumento = StoredProcedure.ConvertToDataTable(listTitDocumentoML);
                StoredProcedure.Merged(dtTitDocumento, "typeCargaDocumento", "spMergedTitDocumento", "typeDocumento");

                DataTable dttitBitacora = StoredProcedure.ConvertToDataTable(listTitBitacora);
                StoredProcedure.Merged(dttitBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");



                Byte[] zip = File.ReadAllBytes(pathZIP + "//" + idBloque + ".zip");
                return zip;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }

        }
        //Pendiente leer configuración y envión de correo....

        public bool ConcluirDocumento(string sDocId, criteriosBusquedaTitulosML criteriosBusquedaTitulosML, string sUserId)
        {
            bool bBandera = false;
            try
            {
                TituloML oTituloML = new TituloML();
                oTituloML.docId = sDocId;
                oTituloML.estDocumentoId = 8;

                bBandera = new TitDocumentoDAL().actualizaEstatusConcluir(oTituloML);

                if (bBandera)
                {
                    titDocumento otitDocumento = new TitDocumentoDAL().getDatostitDocumentoById(sDocId);
                    oTituloML.docId = otitDocumento.docId;
                    oTituloML.docProfesionistaCorreoAlternativo = otitDocumento.docProfesionistaCorreoAlternativo;

                    titConfiguracionConcentradora titConfiguracionConcentradora = new TitConfiguracionConcentradoraDAL().ObtenerRegistroConfiguracionPorId(otitDocumento.conId);
                    if (titConfiguracionConcentradora.conNotificacionProfesionista.GetValueOrDefault())
                    {
                        string[] arrBanderaEnvioCorreo = enviaCorreoProfesionista(oTituloML, criteriosBusquedaTitulosML, sUserId);
                    }

                    int iCantidadDocumentoByConcluir = new TitDocumentoDAL().GetCountDocumentoByConcluir(otitDocumento.solId);

                    if (iCantidadDocumentoByConcluir == 0)
                    {
                        new TitSolicitudDAL().ConcluirSolicitud(otitDocumento.solId);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bBandera;
        }

        public bool ConcluirSolicitud(string sSolId, criteriosBusquedaTitulosML criteriosBusquedaTitulosML, string sUserId)
        {
            bool bBandera = false;
            try
            {
                List<titDocumento> listTitDocumento = new TitDocumentoDAL().ObtenerListaDocumentosFiltros(criteriosBusquedaTitulosML);
                bBandera = new TitDocumentoDAL().ActualizaEstatusList(listTitDocumento);
                if (bBandera)
                {
                    new TitSolicitudDAL().ConcluirSolicitud(sSolId);
                    titDocumento otitDocumento = new TitDocumentoDAL().getDatostitDocumentoById(listTitDocumento.FirstOrDefault().docId);
                    titConfiguracionConcentradora titConfiguracionConcentradora = new TitConfiguracionConcentradoraDAL().ObtenerRegistroConfiguracionPorId(otitDocumento.conId);
                    if (titConfiguracionConcentradora.conNotificacionProfesionista.GetValueOrDefault())
                    {
                        TituloML oTituloML;
                        foreach (var oTitulo in listTitDocumento)
                        {
                            if (oTitulo.estDocumentoId == 5 || oTitulo.estDocumentoId == 8)
                            {
                                oTituloML = new TituloML();
                                oTituloML.docId = oTitulo.docId;
                                oTituloML.docProfesionistaCorreoAlternativo = oTitulo.docProfesionistaCorreoAlternativo;
                                string[] arrBanderaEnvioCorreo = enviaCorreoProfesionista(oTituloML, criteriosBusquedaTitulosML, sUserId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bBandera;
        }

        #endregion

        #region Manejo de información de plantillas
        public titCatPlantilla ObtenertitCatPlantillaById(string planId)
        {
            return new TitCatPlantillaDAL().ObtenertitCatPlantillaById(planId);
        }
        #endregion

        #region Portal Validación
        public TituloElectronico ValidarDocumentoXML(String docXML)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(TituloElectronico));
            XmlReader oXmlReader = XmlReader.Create(GetMemoryStreamXML(docXML));
            TituloElectronico oTituloElectronico = (TituloElectronico)oXmlSerializer.Deserialize(oXmlReader);

            return oTituloElectronico;
        }
        #endregion

        public byte[] GetFileEnvioSEP(string sSolId)
        {
            return new TitSolicitudDAL().GetFileEnvioSEP(sSolId);
        }

        public byte[] GetFileResultadoSEP(string sSolId)
        {
            return new TitSolicitudDAL().GetFileResultadoSEP(sSolId);
        }
        public MemoryStream GetXMLByDocId(string sDocId)
        {
            XmlDocument doc = new XmlDocument();
            string sXMLData = new TitDocumentoDAL().GetXMLByDocId(sDocId);
            doc.Load(new StringReader(sXMLData));

            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);

            xmlStream.Flush();
            xmlStream.Position = 0;

            return xmlStream;
        }

        public MemoryStream GetMemoryStreamXML(string sXMLData)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(sXMLData));

            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);

            xmlStream.Flush();
            xmlStream.Position = 0;

            return xmlStream;
        }
        #region Portal de descarga
        public string[] verificaTokenDescarga(string idDocumento, string tokenDescarga)
        {
            string[] result = new string[2];
            if (tokenDescarga != null && tokenDescarga != "")
            {
                result[0] = new TitTokenDescargaDAL().verificaToken(idDocumento, tokenDescarga).ToString();
                result[1] = result[0] == "True" ? idDocumento : "El código ingresado no es válido, por favor intente nuevamente o solicite un nuevo código.";
            }
            else
            {
                result[0] = "False";
                result[1] = "Debe ingresar un código de descarga.";
            }

            return result;
        }
        #endregion

        public bool ResolverSolicitud(string sSolId, string sOpcion, PerfilML perfilActivo, string sUsuario)
        {
            bool bBandera = false;
            if (sOpcion.Equals("Reenviar"))
            {
                bBandera = ReenviarSolicitud(sSolId, perfilActivo, sUsuario);
            }
            else if (sOpcion.Equals("Sellar"))
            {
                bBandera = EnviarASellado(sSolId);
            }
            else
            {

            }

            return bBandera;
        }
        public bool ReenviarSolicitud(string sSolId, PerfilML perfilActivo, string sUsuario)
        {
            bool bBandera = false;
            try
            {
                titSolicitud oTitSolicitud = new TitSolicitudDAL().ObtenerRegistroSolicitud(sSolId);

                oTitSolicitud.estSolicitudId = 2;
                oTitSolicitud.solFechaEnvioSEP = null;
                oTitSolicitud.solFechaRespuestaSEP = null;
                oTitSolicitud.solArchivoResultadoSEP = null;
                oTitSolicitud.solArchivoEnvioSEP = null;
                oTitSolicitud.solMensajeEnvioSEP = null;
                oTitSolicitud.solMensajeConsultaSEP = null;
                oTitSolicitud.solMensajeResultadoSEP = null;

                bBandera = new TitSolicitudDAL().AgregarActualizarSolicitud(oTitSolicitud);

                string[] arrResultado = ProcesarSolicitud(sSolId, perfilActivo, sUsuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bBandera;
        }

        public bool EnviarASellado(string sSolId)
        {
            bool bBandera = false;
            try
            {
                titSolicitud oTitSolicitud = new TitSolicitudDAL().ObtenerRegistroSolicitud(sSolId);
                oTitSolicitud.estSolicitudId = 10;
                bBandera = new TitSolicitudDAL().AgregarActualizarSolicitud(oTitSolicitud);
                if (bBandera)
                {
                    bBandera = new TitDocumentoDAL().ActualizarDocumentoBySolId(sSolId);
                }
                else
                {
                    bBandera = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bBandera;
        }

        public bool PendienteSellado(string conId)
        {
            try{

                int lista = new TitDocumentoDAL().RegistrosPorEstatus(new List<int>(){ 1,2},conId);

                return (lista >0 ? true : false) ;
            }catch (Exception ex)
            {
                return true;
            }


        }

    }


}
