/// VK MESSAGE
$(function () {
    selectNum();
})

// выделить текст
var canSend = true;
$(".message-form").on('click', function (e) {
    var checkbox = $(this).find(':checkbox'),
        messageContainer = $(this).find('.message-container'),
        isBtn = $(e.target).hasClass('btn-edit') ||
            $(e.target).hasClass('btn-deleate') ||
            $(e.target).hasClass('fa-pencil') ||
            $(e.target).hasClass('fa-trash') ||
            $(e.target).hasClass('message-unhidden') ||
            $(e.target).hasClass('btn-save-text') ||
            $(e.target).is('.btn-tag') || $(e.target).is('.fa-tag') ||
            $(e.target).is('.btn-flink') || $(e.target).is('.fa-link') ||
            $(e.target).is('.btn-user') || $(e.target).is('.fa-user') ||
            $(e.target).is('.btn-external-link') || $(e.target).is('.fa-external-link') ||
            $(e.target).is('.btn-file-text') || $(e.target).is('.fa-file-text') ||
            $(e.target).is('.btn-level-up') || $(e.target).is('.fa-level-up') ||
            $(e.target).is('.thumb-img') ||
            $(e.target).is('textarea'), formData, url,
        id = messageContainer.find('.wall-get-id').text(),
        instagram = $(this).find('.is-instagram').data('isinst');

    if (canSend) {
        if (!isBtn) {
            if (checkbox.prop("checked") === true) {
                url = "/Options/UnselectText/";

                formData = new FormData();
                if (instagram) {
                    formData.append('inst', instagram);
                    formData.append('group_id', $('.vk-group-id').eq(0).text());
                } else {
                    formData.append('Id', id);
                }

                sentVkForm(formData, url, $(this));

                uncheckInput(checkbox, messageContainer);
            } else {
                url = "/Options/SelectText/";

                formData = new FormData();
                if (instagram) {
                    var src = $(this).find('.im-prev .thumb-img').attr('src'),
                        pre = $(this).find('pre'),
                        textarea = $(this).find('textarea'),
                        coments = $(this).find('.message-comments').text(),
                        likes = $(this).find('.message-like').text(),
                        date = $(this).find('.message-date-hidden').text();

                    formData.append('inst', instagram);
                    formData.append('url', src);
                    formData.append('group_id', $('.vk-group-id').eq(0).text());
                    formData.append('coments', coments);
                    formData.append('likes', likes);
                    formData.append('date', date);

                    if (pre) {
                        formData.append('text', pre.text());
                    } else {
                        formData.append('text', textarea.val());
                    }
                } else {
                    formData.append('Id', id);
                }

                sentVkForm(formData, url, $(this));

                checkbox.prop("checked", true);
                messageContainer.css('background-color', 'rgba(200, 200, 200, 0.90)');
            }
        }
    }

    selectNum();
});

function selectNum() {
    var selectedCount = $(':checked')
        .parents('.message-form')
        .length;

    $('#selected-count').text(selectedCount);
}

// удаление
$(".btn-deleate").on('click', function (e) {
    e.stopPropagation();

    deleateMessage($(this));
});

$(".btn-deleate .fa-trash").on('click', function (e) {
    e.stopPropagation();

    deleateMessage($(this).parent());
});

function deleateMessage(that) {
    var perent = that.parents('.message-form'),
        description = perent.find('.message-description'),
        descriptionPre = description.find('pre'),
        messageContainer = perent.find('.message-container'),
        formData, id = perent.find('.wall-get-id').text(),
        url = "/Options/DeleateText/",
        instagram = perent.find('.is-instagram').data('isinst');

    formData = new FormData();
    if (instagram) {
        formData.append('inst', instagram);
    } else {
        formData.append('Id', id);
    }

    sentVkForm(formData, url, perent);

    if (!descriptionPre.is('pre')) {
        textareaToPre(perent, description);
    }

    uncheckInput(perent.find(':checkbox'), messageContainer);

    perent.find('.message-unhidden')
        .show();
    perent.find('.message-hidden')
        .hide();

    selectNum();
}

// Снять удаление с текста
$(".message-unhidden").on('click', function (e) {
    e.stopPropagation();
    var perent = $(this).parents('.message-form'),
        hidden = perent.find('.message-hidden'),
        unhidden = perent.find('.message-unhidden'),
        formData, id = perent.find('.wall-get-id').text(),
        url = "/Options/UndeleateText/";

    formData = new FormData();
    formData.append('Id', id);
    sentVkForm(formData, url);


    unhidden.hide();
    hidden.show();
});

$(".btn-edit, .btn-edit .fa-pencil, .btn-edit fa-floppy-o")
    .on('click', function (e) {
        e.stopPropagation();

        var btn = $('<span>Сохранить</span>')
            .addClass('btn-save-text')
            .addClass('btn')
            .addClass('btn-danger')
            .css('width', '100%')
            .css('margin-bottom', '15px');

        var perent = $(this).parents('.message-form'),
            description = perent.find('.message-description'),
            descriptionPre = description.find('pre');

        if (descriptionPre.is('pre')) {
            var preContent = descriptionPre.text(),
                height = description.outerHeight();

            perent.find('.fa-pencil')
                .removeClass('fa-pencil')
                .addClass('fa-floppy-o');

            description.html($('<textarea></textarea>')
                .addClass('form-control')
                .addClass('form-textarea')
                .css('height', height)
                .val(preContent))
                .append(btn);

        } else {
            textareaToPre(perent, description);
        }
    });

$(".message-form").on('click', '.btn-save-text', function (e) {
    e.stopPropagation();

    var perent = $(this).parents('.message-form'),
        description = perent.find('.message-description');

    textareaToPre(perent, description);
});

// сохранения изменений в тексте
function textareaToPre(perent, description) {
    var text = description.find('textarea').val(),
        id = perent.find('.wall-get-id').text(),
        url = "/Options/ChangeText/",
        instagram = perent.find('.is-instagram').data('isinst');

    var formData = new FormData();
    formData.append('Text', text);
    if (instagram) {
        var group_id = $('.vk-group-id').eq(0).text();

        formData.append('inst', instagram);
        formData.append('group_id', group_id);
    } else {
        formData.append('Id', id);
    }

    sentVkForm(formData, url, perent);

    perent.find('.fa-floppy-o')
        .removeClass('fa-floppy-o')
        .addClass('fa-pencil');

    description.html($('<pre></pre>')
        .text(text));
}

function uncheckInput(checkbox, messageContainer) {
    checkbox.prop("checked", false);
    messageContainer.css('background-color', 'rgba(238, 238, 238, 0.50)');
}

/// HEAD SORTING
$("#message-input").keyup(function () {
    var
        messages = $('.message-container'),
        title, description, group, text, i,
        position;

    for (i = 0; i < messages.length; i++) {
        if (title = messages.eq(i).find('.message-title'))
            text = title.text();
        if (description = messages.eq(i).find('.message-description'))
            text += description.text();
        if (group = messages.eq(i).find('.message-group'))
            text += group.text();
        if (position = messages.eq(i).find('.message-position'))
            text += position.text();

        if (text.length > 0) {
            var filter = new RegExp($("#message-input").val(), 'i');
            messages.eq(i)[filter.test(text) ? 'show' : 'hide']();
        }
    }
});

$("#btn-sort-by-like").on('click', function () {
    vkReclass($(this));
    vkMessageSort('.message-like', $(this));
});

$("#btn-sort-by-share").on('click', function () {
    vkReclass($(this));
    vkMessageSort('.message-share', $(this));
});

$("#btn-sort-by-view").on('click', function () {
    vkReclass($(this));
    vkMessageSort('.message-view', $(this));
});

$("#btn-sort-by-date").on('click', function () {
    vkReclass($(this));
    vkMessageSort('.message-date-hidden', $(this));
});

function vkReclass(id) {
    var child = id.children('i');
    if (child.is('.up')) {
        child.removeClass('up');
        child.addClass('down');
    } else {
        child.addClass('up');
        child.removeClass('down');
    }
}

function vkMessageSort(sortBy, id) {
    var
        messages, messagesF, messagesS, i,
        switching = true, shouldSwitch,
        child = id.children('i');

    while (switching) {
        switching = false;
        messages = $('.message-form');
        for (i = 0; i < (messages.length - 1); i++) {
            shouldSwitch = false;
            messagesF = messages.eq(i).find(sortBy).text();
            messagesS = messages.eq(i + 1).find(sortBy).text();

            if (child.is('.down')) {
                if (parseInt(messagesF) > parseInt(messagesS)) {
                    shouldSwitch = true;
                    break;
                }
            } else {
                if (parseInt(messagesF) < parseInt(messagesS)) {
                    shouldSwitch = true;
                    break;
                }
            }

        }
        if (shouldSwitch) {
            (messages)[i].parentNode.insertBefore(messages[i + 1], messages[i]);
            switching = true;
        }
    }
}

// SEND INFO
var result = $.parseJSON('{ "ok": "error" }');
function sentVkForm(formData, url, that) {
    $.ajax({
        url: url,
        type: 'post',
        data: formData,
        cache: false,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.ok === "error") {
                console.log(data.ok);
            }
            result = data;

            if (that && data.ok === "ok" && data.inst_id) {
                var span = that.find('.is-instagram');

                span.data('isinst', data.inst_id);
                span.attr('data-isinst', data.inst_id);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus);
        }
    });
}

// TO SEND SELECT
$("#send-messages, #send-selected").on('click', function (e) {
    e.stopPropagation();

    var that = $(this);

    if (!that.hasClass('is-disabled')) {
        canSend = false;
        $('#send-messages').addClass('is-disabled');
        $('#send-selected').addClass('is-disabled');
        setTimeout(
            function () {
                $('#send-messages').removeClass('is-disabled');
                $('#send-selected').removeClass('is-disabled');
                canSend = true;
            }, 5000);


        //var visibleContent = $('.message-hidden:not(:hidden)');
        var selectedContent = $(':checked').parents('.message-form'),
            group_id = $('.vk-group-id').eq(0).text(),
            url = "/Options/WallPost/", formData,
            instagram = $('.is-instagram').eq(0).data('isinst');

        //console.log(visibleContent.length);
        //console.log(selectedContent.length);

        formData = new FormData();
        formData.append('group_id', group_id);
        if (instagram) {
            formData.append('inst', instagram);
        }

        //console.log(group_id);
        //console.log(instagram);

        sentVkForm(formData, url);
        /* if (result.ok === "ok") */{
            selectedContent.hide();
            result.ok = "false";
        }
    }
});

$('.deleate-btn-from').on('click', function (e) {
    e.stopPropagation();

    deleateFrom($(this));
});

$('.deleate-btn-from .fa-trash').on('click', function (e) {
    e.stopPropagation();

    deleateFrom($(this).parents('.deleate-btn-from'));
});

function deleateFrom(that) {
    var idFrom = that.data('idfrom'), groupId,
        formData = new FormData(), url = '/Options/DeleateGroupFrom/';

    if (idFrom === 'all') {
        groupId = that.data('group');
        formData.append('group_id', groupId);
        formData.append('all', 'all');

        $('.gf-options').hide();
    } else {
        formData.append('group_id', idFrom);

        that.parents('.gf-options').hide();
    }

    sentVkForm(formData, url);
}


/* GIV HOVER */
$('img.gif-previous').hover(function () {
    $(this).attr('src', $(this).data('gif'));
}, function () {
    $(this).attr('src', $(this).data('prev'));
});


/* КНОПКИ ТЕКСТ РЕДАКТОРЫ 
 * btn-tag fa-tag удалить тэги
 * btn-flink fa-link удалить ссылки
 * btn-user fa-user удалить автора
 * btn-external-link fa-external-link удалить wiki ссылки
 * btn-file-text fa-file-text удалить весь текст
 * btn-level-up fa-level-up восстановить текст
 */

$('.btn-tag, .btn-tag .fa-tag').on('click', function (e) {
    e.stopPropagation();

    var url = "/Options/RemoveTag/";
    replaceText($(this), url);
});

$('.btn-flink, .btn-flink .fa-link').on('click', function (e) {
    e.stopPropagation();

    var url = "/Options/RemoveLink/";
    replaceText($(this), url);
});

$('.btn-user, .btn-user .fa-user').on('click', function (e) {
    e.stopPropagation();

    var url = "/Options/RemoveAuthor/";
    replaceText($(this), url);
});

$('.btn-external-link, .btn-external-link .fa-external-link').on('click', function (e) {
    e.stopPropagation();

    var url = "/Options/RemoveWikiLink/";
    replaceText($(this), url);
});

$('.btn-file-text, .btn-file-text .fa-file-text').on('click', function (e) {
    e.stopPropagation();

    var url = "/Options/RemoveText/";
    replaceText($(this), url);
});

$('.btn-level-up, .btn-level-up .fa-level-up').on('click', function (e) {
    e.stopPropagation();

    var url = "/Options/BackUpText/";
    replaceText($(this), url);
});

function replaceText(that, url) {
    var perent = that.parents('.message-form'),
        description = perent.find('.message-description'),
        id = perent.find('.wall-get-id').text(),
        formData, pre = description.find('pre'),
        textarea = description.find('textarea'),
        instagram = perent.find('.is-instagram').data('isinst');

    formData = new FormData();
    formData.append('text_id', id);
    formData.append('inst', instagram);

    if (pre) {
        if (instagram) {
            formData.append('text', pre.text());
            formData.append('group_id', $('.vk-group-id').eq(0).text());
        }

        sendText(formData, url, pre, true, perent)
    } else {
        if (instagram) {
            formData.append('text', textarea.val());
            formData.append('group_id', $('.vk-group-id').eq(0).text());
        }

        sendText(formData, url, textarea, false, perent)
    }
}

function sendText(formData, url, parant, isPre, that) {
    $.ajax({
        url: url,
        type: 'post',
        data: formData,
        cache: false,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.ok === "ok") {
                if (isPre == true) {
                    parant.text(data.text);
                } else {
                    parant.val(data.text);
                }

                if (data.inst_id) {
                    var span = that.find('.is-instagram');

                    span.data('isinst', data.inst_id);
                    span.attr('data-isinst', data.inst_id);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus);
        }
    });
}



//var perent = $(this).parents('.message-form'),
//    description = perent.find('.message-description');

//    var text = description.find('textarea'),
//        id = perent.find('.wall-get-id').text(),
//        url = "/Options/ChangeText/";

//    var pre = description.find('pre');
//var textarea = description.find('textarea');