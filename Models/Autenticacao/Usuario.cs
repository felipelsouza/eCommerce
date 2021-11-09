using System;

namespace eCommerce.Models.Autenticacao
{
  @inject Microsoft.AspNetCore.Identity.SignInManager<eCommerce.Models.Autenticacao.Usuario> SignInManager
  @inject Microsoft.AspNetCore.Identity.UserManager<eCommerce.Models.Autenticacao.Usuario> UserManager
  {
    if (inject is null)
    {
      throw new ArgumentNullException(nameof(inject));
    }

    if (UserManager is null)
    {
      throw new ArgumentNullException(nameof(UserManager));
    }
  }

  public class Usuario : IdentityUser
  {
  }
}