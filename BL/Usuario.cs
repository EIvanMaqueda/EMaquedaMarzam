using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Login(string nombre,string password) { 
        
            ML.Result result=new ML.Result();
            try
            {
                using (DL.EMaquedaMarzaEntities context=new DL.EMaquedaMarzaEntities())
                {
                    var query = context.Login(nombre,password).AsEnumerable().FirstOrDefault();
                    if (query != null) { 
                     
                        ML.Usuario usuario= new ML.Usuario();
                        usuario.IdUsuario=query.IdUsuario;
                        usuario.Nombre=query.Nombre;
                        usuario.Password = query.Password;
                        usuario.ApellidoPaterno=query.ApellidoPaterno;
                        usuario.ApellidoMaterno=query.ApellidoMaterno;
                        
                        result.Object=usuario;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
                result.Correct=false;
            }
            return result;
        }
    }
}
