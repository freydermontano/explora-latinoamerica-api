using blogExploraLatamAPI.Models.DTO;
using blogExploraLatamAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace blogExploraLatamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }



        // {apibaseUrl}/api/auth/logi
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var isIdentityUser = await userManager.FindByEmailAsync(request.Email);

            if(isIdentityUser is not null)
            {
                //Comprobar el password
               var checkPasswordResult =  await userManager.CheckPasswordAsync(isIdentityUser, request.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(isIdentityUser);

                    //Crear token y respuesta
                    var jwtToken =  tokenRepository.CreateJwtToken(isIdentityUser, roles.ToList());
                    

                    //Mandar respuesta para el cliente
                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken

                    };

                    return Ok(response);   
                }

            }

            ModelState.AddModelError("", "Credenciales Inavlidas");
            return ValidationProblem(ModelState);


        }




        // {apibaseUrl}/api/auth/register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {

            //Crear un nuevo objeto  de tipo IdentityUser 
             var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };

            //Crear un usuario y guarda el usuario en la base de datos.
            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                //Agregar role al usuario (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");


                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }

            return ValidationProblem(ModelState);

        }



    }

  
}


// UserManager es la clase que te da ASP.NET Identity para crear, actualizar, eliminar usuarios, asignar roles, manejar contraseñas, etc.

//UserManager, es la clase oficial de Identity para gestionar usuarios.
//IdentityUser, modelo que representa a un usuario en la BD (tabla AspNetUsers).
//CreateAsync, crea usuario y valida password según reglas configuradas.
//AddToRoleAsync, asigna roles para autorización futura.