jQuery(document).ready(function ($) {

    $('.figure').draggable();
    $(window).on('load', Resize());
    $(window).resize(() => Resize());
    $(window).unload(function () {
        window.localStorage.removeItem("currentTime")
    });
    var userId = $("#userId").val();
    var addTime = 0;
    window.localStorage.setItem("userId", userId);

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/gameChat")
        .build();

    hubConnection.start()
        .then(function () {
            console.log("gameChatStart");
            GetNamesAndTimes();

            var clock = window.localStorage.getItem("currentTime");

            if (clock != null) {

                Start(clock);
            }

            GetLogs();
        })
        .catch(function (err) {
            return console.error(err.toString());
        });


    hubConnection.on("Start", function (gameId) {

        var gameIdCurrent = $("#game-id2").val();

        if (gameId === gameIdCurrent) {

            TimerStart();
        }
    });

    hubConnection.on("Move", function (gameId, list) {

        var gameIdCurrent = $("#game-id2").val();

        if (gameId === gameIdCurrent) {

            if (list == null) {

                Reverse();
                return;
            }

            for (var i = 0; i < list.length; i += 2) {

                var cell = $(`div[name="${list[i + 1]}"]`);
                var figure = $(`.${list[i]}`);

                figure.removeClass(list[i]);
                figure.addClass(list[i + 1]);

                figure.offset({ top: cell.offset().top, left: cell.offset().left });
                $(".visible").removeClass("visible")
                AddLog(list[i], list[i + 1]);
                Switch();
            }
        }
    });

    hubConnection.on("Remove", function (gameId, figureId) {

        var gameIdCurrent = $("#game-id2").val();

        if (gameId === gameIdCurrent) {

            $(`#${figureId}`).remove();
        }
    });

    hubConnection.on("NamesAndTimes", function (gameId, names, times, add) {

        var gameIdCurrent = $("#game-id2").val();

        if (gameId === gameIdCurrent) {

            for (var i = 0; i < names.length; i++) {

                $(`#userName-${i}`).html(names[i]);
                $(`#clock-${i}`).html(times[i]);

                window.localStorage.setItem(`#clock-${i}`, times[i])
            }

            if (!$(".king-white").hasClass("ui-draggable")) {
                $("#clock-0").attr("id", "clock-1");
                $("#clock-1").attr("id", "clock-0");
                $("#userName-0").attr("id", "userName-1");
                $("#userName-1").attr("id", "userName-0");
            }

            addTime = add;
        }
    });

    hubConnection.on("Transform", function (gameId, figureId, figureName) {

        var gameIdCurrent = $("#game-id2").val();

        if (gameId === gameIdCurrent) {

            var oldFigure = window.localStorage.getItem("newCell");
            var classList = $(`.${oldFigure}`).attr("class").split(' ');

            for (var i = 0; i < classList.length; i++) {

                if (classList[i].match("^pawn")) {

                    var color = classList[i].split('-')[1];
                }
            }

            $(`.${oldFigure}`).toggleClass(`pawn-${color} ${figureName}-${color}`);
            $(`.${oldFigure}`).attr("Id", figureId);
        }
    });

    hubConnection.on("Gameover", function (gameId, color) {

        var gameIdCurrent = $("#game-id2").val();

        if (gameId === gameIdCurrent) {

            $(".gameover").addClass("gameover-visible");
 
            if (color == null) {
                var message = $("#draw").html();
            }

            if (color == "White") {
                var message = $("#black").html();
            }

            if (color == "Black") {
                var message = $("#white").html();
            }

            $(".mes").html(message);
            $(".message").addClass("message-visible");
            Stop("#clock-0");
            Stop("#clock-1");
            window.localStorage.removeItem("#clock-0")
            window.localStorage.removeItem("#clock-1")
        }
    });


    $('.figure').on('mousedown', async function (e) {

        var elements = document.elementsFromPoint(e.pageX, e.pageY);
        var oldCell = elements.find(el => el.classList.contains("field-cell")).getAttribute("name");

        window.localStorage.setItem("oldCell", oldCell);
        $(".visible").removeClass("visible")
        await GetPossibleMove(oldCell);
    });

    $('.figure').on('mouseup', async function (e) {

        var field = $('.field-container');

        var fieldTop = field.offset().top;
        var fieldLeft = field.offset().left;

        var fieldWidth = field.width();
        var fieldHeight = field.height();

        var cursorTop = e.pageY - fieldTop;
        var cursorLeft = e.pageX - fieldLeft;

        if ((cursorLeft > 0 && cursorLeft < fieldWidth) && (cursorTop > 0 && cursorTop < fieldHeight)) {

            var elements = document.elementsFromPoint(e.pageX, e.pageY);
            var newCell = elements.find(el => el.classList.contains("field-cell")).getAttribute("name");
            var dot = elements.find(el => el.classList.contains("field-cell")).children.item(0);
            var oldCell = window.localStorage.getItem("oldCell");
            var attack = elements.find(el => el.classList.contains("field-cell")).children.item(1);

            if (dot.classList.contains("visible") || attack.classList.contains("visible")) {

                if ($(this).hasClass("pawn-white")) {

                    if (newCell.split('-')[1] == "8") {
                        window.localStorage.setItem("newCell", newCell);
                        $(".choice").addClass("choice-visible");
                        $(".gameover").addClass("gameover-visible");
                    }
                }

                if ($(this).hasClass("pawn-black")) {
                    if (newCell.split('-')[1] == "1") {
                        window.localStorage.setItem("newCell", newCell);
                        $(".choice").addClass("choice-visible");
                        $(".gameover").addClass("gameover-visible");
                    }
                }

                Move(oldCell, newCell);
            }
            else {

                Reverse();
            }
        }
        else {

            Reverse();
        }
    });

    $("#btn-copy").on("click", function CopyToClipboard() {
        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val($("#game-id").prop('href')).select();
        document.execCommand("copy");
        $temp.remove();

        var alert = $('.alert');
        alert.addClass("alert-visible");
        alert.animate({
            opacity: 0,
        }, 1500, "linear", function () {
            $(this).removeClass("alert-visible");
            $(this).css({ "opacity": "1" });
        });
    });

    $('.figure-choice').on('click', async function (e) {

        var figure = $(this).attr("class").split(' ')[0].split('-')[0];
        $(".choice").removeClass("choice-visible");
        var newCell = window.localStorage.getItem("newCell");
        var gameId = $("#game-id2").val();
        $.get(`/api/game/SwitchPawn/${gameId}/${newCell}/${figure}`);
        $(".gameover").removeClass("gameover-visible");
    });

    $('#leave').on('click', async function (e) {

        var gameId = $("#game-id2").val();
        var id = window.localStorage.getItem("userId");
        $.get(`/api/Game/LeaveGame/${gameId}/${id}`);
    });


    function Stop(id) {

        $(id).countdown("stop");
        var time = $(id).html().split(":")
        var mm = Number(time[0]);
        var ss = Number(time[1]);
        ss += addTime;

        if (ss >= 60) {
            ss -= 60;
            mm += 1;
        }

        var nextTime = `${mm}:${ss}`;
        if (ss < 10) {
            var nextTime = `${mm}:0${ss}`;
        }

        $(id).html(nextTime)
        window.localStorage.setItem(id, nextTime)
    }

    function Start(id) {

        var newTime = window.localStorage.getItem(id).split(":");
        var mm = Number(newTime[0]);
        var ss = Number(newTime[1]);
        var date3 = new Date(new Date().valueOf() + (mm * 60 + ss) * 1000);

        $(id).countdown(date3, function (event) {
            $(this).html(event.strftime('%M:%S'));
        }).countdown('start');
    }

    var check = false;
    function Switch() {
        if (check) {

            window.localStorage.setItem("currentTime", "#clock-1");
            Stop("#clock-0");
            Start("#clock-1");
            check = false;
        } else {

            window.localStorage.setItem("currentTime", "#clock-0");
            Stop("#clock-1");
            Start("#clock-0");
            check = true;
        }
    }

    function TimerStart() {

        var m = Number(window.localStorage.getItem("#clock-0").split(':')[0]);
        var m2 = Number(window.localStorage.getItem("#clock-1").split(':')[0])

        var date = new Date(new Date().valueOf() + m * 60 * 1000);

        $('#clock-0').countdown(date, function (event) {
            $(this).html(event.strftime('%M:%S'));
        }).countdown("stop");

        var date2 = new Date(new Date().valueOf() + m2 * 60 * 1000);

        $('#clock-1').countdown(date2, function (event) {
            $(this).html(event.strftime('%M:%S'));
        }).countdown("stop");

        Start("#clock-0");
        check = true;
        window.localStorage.removeItem("currentTime");
        window.localStorage.setItem("currentTime", "#clock-0");
    }

    function Resize() {
        var fidures = $(".figure");
        var figudesEnemy = $(".figure-enemy");
        var list = $.merge(fidures, figudesEnemy);

        for (let i = 0; i < list.length; i++) {
            var classes = list[i].classList;
            for (var j = 0; j < classes.length; j++) {

                if (classes[j].length === 3) {

                    var cell = classes[j];
                    break;
                }
            }

            $(`.${cell}`).offset({
                top: $(`[name="${cell}"]`).position().top,
                left: $(`[name="${cell}"]`).position().left
            });
        }
    }

    function Reverse() {

        var name = window.localStorage.getItem("oldCell");
        var figure = $(`.${name}`);
        var cell = $(`div[name="${name}"]`);
        figure.offset({ top: cell.offset().top, left: cell.offset().left });
    }

    function GetPossibleMove(currentCellName) {

        var id = window.localStorage.getItem("userId");

        $.get(`/api/Game/GetPossibleMove/${id}/${currentCellName}`, function (result) {

            ViewMove(result);
        });
    }

    function Move(oldCell, newCell) {

        var id = window.localStorage.getItem("userId");
        $.get(`/api/Game/Move/${id}/${oldCell}/${newCell}`);
    }

    function ViewMove(listCell) {

        for (let i = 0; i < listCell.length; i++) {

            var child = $(`[name="${listCell[i]}"]`).children(".field-cell-circle");
            var figure = $(`.${listCell[i]}`);

            if (!child.hasClass("visible")) {
                child.addClass("visible");
            }

            if (figure.length != 0) {

                var childAttact = $(`[name="${listCell[i]}"]`).children(".attack");
                child.removeClass("visible");
                childAttact.addClass("visible");
            }
        }
    }

    var num = -1;
    function AddLog(oldCell, newCell) {

        UpdateLogs();

        if (num === -2) {
            num = -1;
        }
        else {
            num = -2;
        }
        $('.log-message').eq(num).append(`<label class="log-text">${oldCell} &#8594 ${newCell}</label>`);
    };

    function UpdateLogs() {

        if (num === -1) {
            var i = $('.log-container').length;

            $('#logs').append(`<div class="log-container flex"><div class="log-index"><label class="index">${i + 1}</label></div></div>`);

            $('.log-container').last().append('<div class="log-message"></div>');
            $('.log-container').last().append('<div class="log-message"></div>');
        }
    }

    function GetLogs() {
        var gameId = $("#game-id2").val();

        $.get(`/api/Game/GetLogs/${gameId}`, function (result) {

            for (var i = result.length - 1; i >= 0; i--) {

                var array = result[i].message.split(' ');
                AddLog(array[0], array[2]);
            }
        });
    }

    function GetNamesAndTimes() {
        var gameId = $("#game-id2").val();

        $.get(`/api/Game/GetNamesAndTimes/${gameId}`);
    }
});