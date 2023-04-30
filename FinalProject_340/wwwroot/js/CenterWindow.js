window.addEventListener('resize', function () {
    centerWindow("control_btns");
});
function centerWindow(element_id) {
    let w = window.innerWidth;
    let h = window.innerHeight;

    let element = document.getElementById(element_id);

    let elementWidth = element.getBoundingClientRect().width;
    let elementHeight = element.getBoundingClientRect().height;

    console.log(w + " " + h + " " + elementWidth);

    //element.style.top = (innerHeight * .25) + "px";
    element.style.left = ((innerWidth - elementWidth) * .5) + "px";

}