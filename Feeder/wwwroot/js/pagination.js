var current_page = 1;
records_per_page = 10;

function SetItems(obj) {
    items = obj;
}

function Previos() {
    if (current_page > 1) {
        current_page--;
    }
}

function Next() {
        current_page++;
}


$("#next").click(function () {
    $("#loader").css("display", "block");
    Next();
    $.ajax({
        method: 'get',
        url: `Feeder/GetAllItems?page=${current_page}`,
        success: function (result) {
            $('#items').empty();
            $('#items').html(result);
        },
        error: function () {
            Previos();
            $("#next").toggle(false);
        }
    });
    $("#loader").css("display", "none");
});


$("#previos").click(function () {
    $("#loader").css("display", "block");
    $("#next").toggle(true);
    if (current_page > 1) {
        Previos();
        $.ajax({
            method: 'get',
            url: `Feeder/GetAllItems?page=${current_page}`,
            success: function (result) {
                $('#items').empty();
                $('#items').html(result);
            }
        })
    }
    $("#loader").css("display", "none")
});
