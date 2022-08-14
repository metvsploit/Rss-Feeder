$("#tapes").click(function () {
    $("#loader").css("display", "none");
    $.ajax({
        method: 'get',
        url: 'Feeder/Tapes',
        success: function (result) {
            $('#maintain').empty();
            $('#maintain').html(result);
            $("#loader").css("display", "none");
        },
        error: function () {
            alert("Неверно сконфигурирован файл настроек")
            $("#loader").css("display", "none");
        }
    })
});

$("#channels").click(function () {
    $('#maintain').empty();
    $("#loader").css("display", "block");
    $.ajax({
        method: 'get',
        url: 'Feeder/GetAllItems',
        success: function (result) {
            $('#maintain').empty();
            $('#maintain').html(result);
            $("#loader").css("display", "none");
        },
        error: function () {
            alert("Неверно сконфигурирован файл настроек")
            $("#loader").css("display", "none");
        }
    })

});

$("#setting").click(function () {
    $("#loader").css("display", "block");
    $.ajax({
        method: 'get',
        url: 'Setting',
        success: function (result) {
            $('#maintain').empty();
            $('#maintain').html(result);
            $("#loader").css("display", "none");
        },
        error: function () {
            alert("Неверно сконфигурирован файл настроек")
            $("#loader").css("display", "none");
        }
    })
});

