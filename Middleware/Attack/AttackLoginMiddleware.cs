namespace IronDomeAPI.Middleware.Attack
{
    public class AttackLoginMiddleware
    {
        private readonly RequestDelegate _next;
        public AttackLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            Console.WriteLine($"inside AttackLoginMiddleware" );
            await this._next(context);
        }
    }
}
