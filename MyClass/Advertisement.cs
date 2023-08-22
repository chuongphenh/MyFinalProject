using MyFinalProject.Models;

namespace MyFinalProject.MyClass
{
    public class Advertisement
    {
        // Update the return type of the method to match the correct Adv class
        public static List<Adv> GetAdv(int _Position)
        {
            FinalDotnetProjectContext db = new FinalDotnetProjectContext();
            var list_record = db.Advs
                .Where(item => item.PositionAdv == _Position)
                .OrderByDescending(item => item.IdAdv)
                .ToList(); // No need to specify the type explicitly
            return list_record;
        }
    }
}
