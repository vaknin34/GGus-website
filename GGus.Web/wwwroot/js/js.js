var slideIndex = 1;
//showSlides(slideIndex);

// Next/previous controls
function plusSlides(container, n = 0) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function nextSlide(container) {

}

function showSlides(container, n = 0) {
    var i;
    var slides = document.getElementsByClassName(container);
    var dots = document.getElementsByClassName("demo");
    var captionText = document.getElementById("caption");
    var slideIndex = 1;

    if (n > slides.length) {
        slideIndex = 1;
    }

    if (n < 1) {
        slideIndex = slides.length;
    }

    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }

    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }

    // TODO: overflow / outofboundries
    slides[slideIndex - 1].style.display = "block";
    //dots[slideIndex - 1].className += " active";
    //captionText.innerHTML = dots[slideIndex - 1].alt;
}

//exmaple
class Slider {
    container = "";

    constructor(container) {
        this.container = container;
    }

    nextSlide() {

    }

}
