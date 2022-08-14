let tape = {
    channel: "channel",
    address: "address",
    enabled: true
}

let proxy = {
    url: "default",
    username: "name",
    password: "qwerty"
}

function Remove(name) {
    $.ajax({
        method: 'get',
        url: `Setting/RemoveChannel?name=${name}`,
        success: function (response) {
            $(`#${name}`).remove()
            alert(response.message)
        },
        error: function (response) {
            alert("Ошибка запроса")
        }
    })
}

function AddChannel() {
    tape.channel = $("#channel_name").val();
    tape.address = $("#channel_address").val();

    $.ajax({
        method: 'post',
        url: `Setting/AddChannel`,
        data: tape,
        success: function (response) {
            alert(response.message)
            if (response.success)
            $('#channel_table tr:last').after(`<tr id="${tape.channel}"><td>
                                        <input style="margin-left:20px;" checked type="checkbox">
                                        </td><td>${tape.channel}</td><td>${tape.address}</td>
                                        <td>
                   <button type="button" class="btn btn-danger" onclick="Remove('${tape.channel}')">Удалить</button>
                      </td></tr>`);
        },
        error: function (response) {
            alert("Ошибка запроса")
        }
    })
}

$(document).on('click', '#btn_updateTime', function (event) {
    var time = $("#time_text").val()
    $.ajax({
        method: 'get',
        url: `Setting/UpdateTime?time=${time}`,
        success: function (response) {
            alert(response.message)
        },
        error: function () {
            alert("Ошибка запроса")
        }
    })
});

function EnabledChannel(channel) {
    tape.channel = channel;
    if ($(`#check_${channel}`).is(':checked')) {
        tape.enabled = true
    }
    else {
        tape.enabled = false
    }

    $.ajax({
        method: 'post',
        url: `Setting/EnabledChannel`,
        data: tape,
        success: function (response) {
            alert(response.message)
        },
        error: function () {
            alert("Ошибка запроса")
        }
    })
}

function ChangeFormatByTags() {
    var isFormat = $(`#check_format`).is(':checked');
    $.ajax({
        method: 'get',
        url: `Setting/ChangeFormatByTags?isFormat=${isFormat}`,
        success: function (response) {
            alert(response.message)
        },
        error: function () {
            alert("Ошибка запроса")
        }
    })
}

function ChangeProxyData() {
    proxy.url = $("#proxy_address").val()
    proxy.username = $("#proxy_name").val()
    proxy.password = $("#proxy_pass").val()

    $.ajax({
        method: 'post',
        url: `Setting/ChangeProxyData`,
        data: proxy,
        success: function (response) {
            alert(response.message)
        },
        error: function (response) {
            alert("Ошибка запроса")
        }
    })
}

$("#on_proxy").click(function () {
    $.ajax({
        method: 'get',
        url: `Setting/EnableProxy?isEnabled=true`,
        success: function (response) {
            alert(response.message)
        },
        error: function () {
            alert("Ошибка запроса")
        }
    })
});

$(document).on('click', '#off_proxy', function (event) {
    $.ajax({
        method: 'get',
        url: `Setting/EnableProxy?isEnabled=false`,
        success: function (response) {
            alert(response.message)
            $('#table_proxy td').eq(3).html(`<button id="on_proxy" type="button" class="btn btn-primary">Включить</button>`);
        },
        error: function () {
            alert("Ошибка запроса")
        }
    })
});

$(document).on('click', '#on_proxy', function (event) {
    $.ajax({
        method: 'get',
        url: `Setting/EnableProxy?isEnabled=true`,
        success: function (response) {
            alert(response.message)
            $('#table_proxy td').eq(3).html(`<button id="off_proxy" type="button" class="btn btn-danger">Выключить</button>`);
        },
        error: function () {
            alert("Ошибка запроса")
        }
    })
});

