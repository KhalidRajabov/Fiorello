using FrontToBack.Models;
using System.Collections.Generic;

namespace FrontToBack.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders{ get; set; }

        public SliderContent SliderContent { get; set; }

        public List<Category> Categories { get; set; }

        public List<Product> Products { get; set; }

        public List<Employees> Employees { get; set; }

        public List<Blogs> Blogs { get; set; }
        public List<AuthorSlider> AuthorSliders { get; set; }

        public List<BottomSlider> BottomSliders { get; set; }
    }
}
