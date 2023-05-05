using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Medicamento
    {
        public static ML.Result GetAll() { 
        
            ML.Result result = new ML.Result();
            try
            {
                using (DL.EMaquedaMarzaEntities context=new DL.EMaquedaMarzaEntities())
                {
                    var query = context.MedicamentoGetAll().ToList();
                    if (query!=null)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in query)
                        {
                            ML.Medicamento medicamento = new ML.Medicamento();
                            medicamento.IdMedicamento = obj.IdMedicamento;
                            medicamento.Nombre = obj.Nobre;
                            medicamento.Descripcion=obj.Descripcion;
                            medicamento.Precio = obj.Precio.Value;
                            medicamento.Imagen = obj.Imagen;

                            result.Objects.Add(medicamento);
                        }
                        result.Correct = true;

                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct=false;
                result.Message = "Error: "+ex.Message;
            }
            return result;
        }

        public static ML.Result Add(ML.Medicamento medicamento) { 
        
            ML.Result result = new ML.Result();
            try
            {
                using (DL.EMaquedaMarzaEntities context=new DL.EMaquedaMarzaEntities())
                {
                    int query = context.MedicamentoAdd(medicamento.Nombre,medicamento.Precio,medicamento.Imagen,medicamento.Descripcion);
                    if (query>0)
                    {
                        result.Correct = true;
                        result.Message = "Mediacamento Agregado Exitosamente";
                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct= false;
                result.Message=ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(int IdMedicamento) {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.EMaquedaMarzaEntities context=new DL.EMaquedaMarzaEntities())
                {
                    var query = context.MedicamentoGetById(IdMedicamento).AsEnumerable().FirstOrDefault();
                    if (query!=null)
                    {
                        ML.Medicamento medicamento = new ML.Medicamento();
                        medicamento.IdMedicamento = query.IdMedicamento;
                        medicamento.Nombre = query.Nobre;
                        medicamento.Descripcion = query.Descripcion;
                        medicamento.Precio = query.Precio.Value;
                        medicamento.Imagen = query.Imagen;

                        result.Object = medicamento;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Correct = false;
                
            }
            return result;
        }

        public static ML.Result Update(ML.Medicamento medicamento) { 
        
            ML.Result result = new ML.Result();
            try
            {
                using (DL.EMaquedaMarzaEntities context=new DL.EMaquedaMarzaEntities())
                {
                    int query = context.MedicamentoUpdate(medicamento.Nombre,medicamento.Precio,medicamento.Imagen,medicamento.Descripcion,medicamento.IdMedicamento);
                    if (query>0)
                    {
                        result.Correct = true;
                        result.Message = "Medicamento Actualizado Correctamente";
                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct= false;
                result.Message = ex.Message;
            }
            return result;


        }

        public static ML.Result Delete(int IdMedicamento) { 
        
            ML.Result result = new ML.Result();
            try
            {
                using (DL.EMaquedaMarzaEntities context= new DL.EMaquedaMarzaEntities())
                {
                    int query = context.MediacamentoDelete(IdMedicamento);
                    if (query>0)
                    {
                        result.Correct = true;
                        result.Message = "Medicamento eliminado correctamente";
                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct= false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Usuario Carrito(ML.Usuario usuario, ML.Medicamento medicamento)
        {

            ML.Usuario usertemp = new ML.Usuario();
            usertemp.Medicamentos = new List<object>();
            ML.Medicamento medicamento1 = new ML.Medicamento();
            bool bandera = false;
            int pos = 0;
            foreach (ML.Medicamento medicamentotemp in usuario.Medicamentos)
            {
                if (medicamentotemp.Cantidad==0)
                {
                    medicamentotemp.Cantidad= 1;
                }
                if (medicamentotemp.IdMedicamento == medicamento.IdMedicamento)
                {
                    bandera = true;
                    medicamentotemp.Cantidad +=1;
                    medicamento1 = medicamentotemp;
                    break;
                }
                pos += 1;
            }
            if (bandera == true)
            {
                usuario.Medicamentos[pos] = medicamento1;
            }
            else {
                medicamento.Cantidad= 1;
                usuario.Medicamentos.Add(medicamento);
            }
            return usuario;
        }
    }
}
