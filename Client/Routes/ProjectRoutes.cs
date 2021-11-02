namespace Client.Routes
{
   public static class ProjectRoutes
    {
        public static string GetAll = "/api/Projects";
        public static string GetProject(int id) 
        {
           return "/api/Projects/{id}";
        }
    }
}