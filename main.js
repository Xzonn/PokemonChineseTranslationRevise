$(function () {
    $(".xz-pokemon-text-form").on("submit", function (e) {
        e.preventDefault();
        $(this).find("input")[0].checkValidity() && (location.href = location.origin + location.pathname.replace(".html", "/" + $(this).find("input").val().padStart(3, "0") + ".html"));
    });
});