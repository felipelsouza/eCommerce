namespace eCommerce.Controllers
{
  [Authorize]

  public class AutenticacaoController : Controller
  {
    private readonly eCommerceContext _context;

    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly ILogger _logger;

    public AutenticacaoController(eCommerceContext context,
      UserManager<Usuario> userManager,
      SignInManager<Usuario> signInManager,
      ILogger<AutenticacaoController> logger)
    {
      _context = context;
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Acessar(string returnUrl = null)
    {
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
      ViewData["ReturnUrl"] = returnUrl;
      return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult RegistrarNovoUsuario(string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      return View();
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      else
      {
        return RedirectToAction(nameof(HomeController.Index), "Home");
      }
    }

    public async Task<IActionResult> Sair()
    {
      await _signInManager.SignOutAsync();
      _logger.LogInformation("Usuário realizou logout.");
      return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Acessar(AcessarViewModel model, string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      if (ModelState.IsValid)
      {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, model.LembrarDeMim, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          _logger.LogInformation("Usuário Autenticado.");
          return RedirectToLocal(returnUrl);
        }
      }

      ModelState.AddModelError(string.Empty, "Falha na tentativa de login.");
      return View(model);
    }
  }
}