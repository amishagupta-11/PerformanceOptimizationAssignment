document.addEventListener("DOMContentLoaded", function () {
    var homeImage = document.getElementById("homeImage");

    homeImage.addEventListener("click", function () {
        alert("Image name: " + homeImage.alt);
    });
});
