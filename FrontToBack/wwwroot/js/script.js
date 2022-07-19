$(document).ready(function () {


    //search product

    $(document).on("keyup", "#input-search", function () {
        let inputValue = $(this).val();
        $("#SearchList li").slice(1).remove();
        $("#SearchList").html()
        $.ajax({
            url: "home/searchProduct?search="+inputValue,
            method: "get",
            success: function (res) {
                $("#SearchList").append(res);
            }
        })
    })



    //add product to basket

    let addBtn = document.querySelectorAll(".add")
    let bTotal = document.getElementById("basketTotal")
    let tPrice = document.getElementById("basketPrice")
    addBtn.forEach(add =>
        
        add.addEventListener("click", function () {
            let dataId = this.getAttribute("data-id")
            console.log(dataId)
            axios.post("/basket/additem?id=" + dataId)
                .then(function (response) {
                    // handle success
                    bTotal.innerHTML = response.data.count
                    tPrice.innerHTML = ` $${response.data.price}`
                    //console.log(response);
                })
                .catch(function (error) {
                    // handle error
                    alert("error"+ dataId)
                    console.log(error);
                })
        })
        )

    //plus item in basket

    let plusBtn = document.querySelectorAll(".plusitem")
    plusBtn.forEach(add =>

        add.addEventListener("click", function () {
        
            let dataId = this.getAttribute("data-id")
            let span = this.previousElementSibling;
            let tabletotalprice = this.parentElement.previousElementSibling;
            console.log(dataId)
            axios.post("/basket/plus?id=" + dataId)
                .then(function (response) {
                    
                    // handle success
                    bTotal.innerText = response.data.count
                    tPrice.innerText = response.data.price
                    span.innerText = response.data.main
                    tabletotalprice.innerText=response.data.itemTotal
                    console.log(response)
                    console.log(response.data.main)
                    //console.log(response);
                })
                .catch(function (error) {
                    // handle error
                    
                    console.log(error);
                })
        })
    )


    //minus item in basket


    let minusBtn = document.querySelectorAll(".minusitem")
    minusBtn.forEach(add =>
        add.addEventListener("click", function () {
        
            let dataId = this.getAttribute("data-id")
            let span = this.nextElementSibling
            let tr = span.parentElement.parentElement;
            let tabletotalprice = this.parentElement.previousElementSibling;
            console.log(tr)
            axios.post("/basket/minus?id=" + dataId)
                .then(function (response) {

                    
                    
                    if (response.data.count == 0) {
                        bTotal.innerText = response.data.main
                        tPrice.innerText = response.data.price
                        tr.remove();
                    }
                    else {
                        bTotal.innerText = response.data.main
                        tPrice.innerText = response.data.price
                        span.innerText = response.data.count
                        tabletotalprice.innerText = response.data.itemTotal;
                    }
                    //console.log(response);
                })
                .catch(function (error) {
                    // handle error

                        //tr.remove();
                    

                    console.log(error);
                })
        })
    )


    //delete item in basket


    let delBtn = document.querySelectorAll(".deleteitem")
        delBtn.forEach(add =>
        
        add.addEventListener("click", function () {

            let dataId = this.getAttribute(`data-id`)
            let tr = this.parentElement.parentElement;
            console.log(dataId)
            axios.post("/basket/RemoveItem?id=" + dataId)
                .then(function (response) {

                    
                    bTotal.innerText = response.data.count;
                    tPrice.innerText = response.data.price;
                    tr.remove();
                })
                .catch(function (error) {

                    console.log(error);
                })
        })
        )


    




    // HEADER

    $(document).on('click', '#search', function () {
        $(this).next().toggle();
    })

    $(document).on('click', '#mobile-navbar-close', function () {
        $(this).parent().removeClass("active");

    })

    let skip = 2;
    $(document).on('click', '#loadmore', function () {
        let productCount = $(`#productCount`).val()
        let productList = $("#productList")
        $.ajax({
            url: "/product/loadmore?skip=" + skip,
            method: "get",
            success: function (res) {
                productList.append(res)
                skip += 2;
                if (skip >= productCount) {
                    $(`#loadmore`).remove();
                }
            }
        })

    })


    $(document).on('click', '#mobile-navbar-show', function () {
        $('.mobile-navbar').addClass("active");

    })

    $(document).on('click', '.mobile-navbar ul li a', function () {
        if ($(this).children('i').hasClass('fa-caret-right')) {
            $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down')
        }
        else {
            $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right')
        }
        $(this).parent().next().slideToggle();
    })

    // SLIDER

    $(document).ready(function(){
        $(".slider").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });

    // PRODUCT

    $(document).on('click', '.categories', function(e)
    {
        e.preventDefault();
        $(this).next().next().slideToggle();
    })

    $(document).on('click', '.category li a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.product-item');
        
        products.each(function () {
            if(category == $(this).attr('data-id'))
            {
                $(this).parent().fadeIn();
            }
            else
            {
                $(this).parent().hide();
            }
        })
        if(category == 'all')
        {
            products.parent().fadeIn();
        }
    })

    // ACCORDION 

    $(document).on('click', '.question', function()
    {   
       $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
       $(this).siblings('.answer').not($(this).next()).slideUp();
       $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
       $(this).next().slideToggle();
       $(this).siblings('.active').removeClass('active');
       $(this).toggleClass('active');
    })

    // TAB

    $(document).on('click', 'ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().next().children('p.active').removeClass('active');

        $(this).parent().next().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    $(document).on('click', '.tab4 ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    // INSTAGRAM

    $(document).ready(function(){
        $(".instagram").owlCarousel(
            {
                items: 4,
                loop: true,
                autoplay: true,
                responsive:{
                    0:{
                        items:1
                    },
                    576:{
                        items:2
                    },
                    768:{
                        items:3
                    },
                    992:{
                        items:4
                    }
                }
            }
        );
      });

      $(document).ready(function(){
        $(".say").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });
})
