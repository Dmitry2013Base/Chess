$(".setting-item").on("click", function () {

    var times = $(this).children(".setting-game").children(".game-item").eq(1).children("p").html().split('+');
    window.open(`/api/Game/CreateGame/${times[0]}/${times[1]}`, "_blank")
});