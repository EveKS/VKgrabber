$(function () {
    $('.gic-btn .fa-trash').on('click', function (e) {
        e.stopPropagation();

        var url = '/Options/DeleateGroup/', formData = new FormData(),
            groupId = $(this).parents('.gic-btn').data('group');
        formData.append('group_id', groupId);
        sentVkForm(formData, url);

        $(this).parents('.group-info-body').hide();
    });
});

$(".filter-btn-from").on('click', function (e) {
    e.stopPropagation();

    var that = $(this);

    setValue(that);
});

$(".filter-btn-from .fa-cog").on('click', function (e) {
    e.stopPropagation();

    var that = $(this).parents('.filter-btn-from');

    setValue(that);
});

function setValue(that) {
    var options = that.data('options'),
        tags = options.at, group = options.gr, all = options.all,
        replace1 = options.rf1, rto1 = options.rt1,
        replace2 = options.rf2, rto2 = options.rt2,
        link = options.gwl, pick = options.gwp,
        vklink = options.gwvl, wiki = options.gwwp,
        txt = options.rtxt, removetag = options.rt,
        gpost = options.ggp, author = options.ra,
        copia = options.cwa, rs = options.rs;

    var textarea = $('#modal-option-new-groups').find('textarea');
    textarea.val(tags);

    $('#modal-option-new-groups').find('input.group-id-tag:hidden')
        .val(group);
    $('#modal-option-new-groups').find('input.group-id-all:hidden')
        .val(all);

    $('#modal-option-new-groups').find('input[name="RepalaceFrom1"]')
        .val(replace1);
    $('#modal-option-new-groups').find('input[name="RepalaceTo1"]')
        .val(rto1);
    $('#modal-option-new-groups').find('input[name="RepalaceFrom2"]')
        .val(replace2);
    $('#modal-option-new-groups').find('input[name="RepalaceTo2"]')
        .val(rto2);
    $('#modal-option-new-groups').find('input[name="GetWithLink"]')
        .prop('checked', isCheck(link));
    $('#modal-option-new-groups').find('input[name="GetWithPicture"]')
        .prop('checked', isCheck(pick));
    $('#modal-option-new-groups').find('input[name="GetWithVkLink"]')
        .prop('checked', isCheck(vklink));
    $('#modal-option-new-groups').find('input[name="GetWithWikiPage"]')
        .prop('checked', isCheck(wiki));
    $('#modal-option-new-groups').find('input[name="RemoveText"]')
        .prop('checked', isCheck(txt));
    $('#modal-option-new-groups').find('input[name="RemoveTag"]')
        .prop('checked', isCheck(removetag));
    $('#modal-option-new-groups').find('input[name="GetOnlyGroupPost"]')
        .prop('checked', isCheck(gpost));
    $('#modal-option-new-groups').find('input[name="RemoveAuthor"]')
        .prop('checked', isCheck(author));
    $('#modal-option-new-groups').find('input[name="CopyWithAuthor"]')
        .prop('checked', isCheck(copia));
    $('#modal-option-new-groups').find('input[name="RemoveSmile"]')
        .prop('checked', isCheck(rs));

    showHide();
}

function showHide() {
    // открываем только после закрытия
    $('#modal-add-groups').modal('hide');

    $("#modal-add-groups").on('hidden.bs.modal', function () {
        $('#modal-option-new-groups').modal('show');
        $(this).off('hidden.bs.modal');
    });
}

function isCheck(prop) {
    return prop === true || prop === "True" || prop === "true";
}

$('#modal-option-new-groups form').on('submit', function (e) {
    e.preventDefault();
    e.stopPropagation();

    var that = $(this), id = that.find('input.group-id-tag:hidden').val(),
        rf1 = that.find('input[name="RepalaceFrom1"]'),
        rt1 = that.find('input[name="RepalaceTo1"]'),
        rf2 = that.find('input[name="RepalaceTo2"]'),
        gwl = that.find('input[name="GetWithLink"]'),
        gwp = that.find('input[name="GetWithPicture"]'),
        gwvl = that.find('input[name="GetWithVkLink"]'),
        gwwp = that.find('input[name="GetWithWikiPage"]'),
        rtxt = that.find('input[name="RemoveText"]'),
        rt = that.find('input[name="RemoveTag"]'),
        ggp = that.find('input[name="GetOnlyGroupPost"]'),
        ra = that.find('input[name="RemoveAuthor"]'),
        cwa = that.find('input[name="CopyWithAuthor"]'),
        rs = that.find('input[name="RemoveSmile"]');

    var newJson = '{"at":"' + that.find('textarea').val() +
        '","gr":"' + id + '","all":"' +
        that.find('input.group-id-all:hidden').val() + '","rf1":"'
        + rf1.val() + '","rt1":"' + rt1.val() + '","rf2":"' +
        that.find('input[name="RepalaceFrom2"]').val() + '","rt2":"'
        + rf2.val() + '","gwl":"' + gwl.prop('checked') + '","gwp":"' +
        gwp.prop('checked') + '","gwvl":"' + gwvl.prop('checked') + '","gwwp":"' +
        gwwp.prop('checked') + '","rtxt":"' + rtxt.prop('checked') + '","rt":"'
        + rt.prop('checked') + '","ggp":"' + ggp.prop('checked') + '","ra":"' +
        ra.prop('checked') + '","cwa":"' + cwa.prop('checked') + '","rs":"'
        + rs.prop('checked') + '"}';

    var find = 'label.filter-btn-from[data-idfrom="' + id + '"]';
    $(find).data('options', $.parseJSON(newJson));
    $(find).attr('data-options', newJson);

    var $form = $(this), url = $form.attr('action'),
        formData = new FormData($form.get(0));

    sentVkForm(formData, url);
    $('#modal-option-new-groups').modal('hide');
});

$('.group-info-container .fa-cogs').on('click', function (e) {
    e.stopPropagation();

    var that = $(this), group_id = that.data('group'),
        select_from = that.data('timef'),
        select_to = that.data('timet'),
        max_froam = that.data('maxfrom'),
        max_load = that.data('maxload'),
        modal_form = $('#modal-vkgroups form');

    modal_form.find('input[name="group_id"]').val(group_id);
    modal_form.find('option').prop('selected', false);

    var options_from = modal_form.find('select[name="select_from"]')
        .find('option');
    var options_to = modal_form.find('select[name="select_to"]')
        .find('option');

    if (select_from !== "" && select_to !== "" && select_from && select_to) {
        selectOptions(options_from, select_from);
        selectOptions(options_to, select_to);
    } else {
        selectOptions(options_from, '30');
        selectOptions(options_to, '30');
    }

    var options_maxfrom = modal_form.find('.value-block input[name="max_from"]');
    var options_maxload = modal_form.find('.value-block input[name="max_load"]');

    if (max_load !== "" && max_load) {
        options_maxload.val(max_load);
    } else {
        options_maxload.val('50');
    }

    if (max_froam !== "" && max_froam) {
        options_maxfrom.val(max_froam);
    } else {
        options_maxfrom.val('30');
    }

    modal_form.parents('#modal-vkgroups').modal('show');
});

function selectOptions(opt, number) {
    for (var i = 0; i < opt.length; i++) {
        if (opt.eq(i).attr('value') == number) {
            opt.eq(i).prop('selected', true)
        }
    }
}

$('#modal-vkgroups form').on('submit', function (e) {
    e.preventDefault();
    e.stopPropagation();

    var that = $(this),
        group_id = that.find('input[name="group_id"]').val(),
        select_from = that.find('select[name="select_from"] option:selected').attr('value'),
        select_to = that.find('select[name="select_to"] option:selected').attr('value'),
        max_froam = that.find('input[name="max_from"]').val(),
        max_load = that.find('input[name="max_load"]').val();

    var find = '.group-info-container i.fa-cogs[data-group="' + group_id + '"]';
    $(find).data('timef', select_from);
    $(find).attr('data-timef', select_from);

    $(find).data('timet', select_to);
    $(find).attr('data-timet', select_to);

    $(find).data('maxfrom', max_froam);
    $(find).attr('data-maxfrom', max_froam);

    $(find).data('maxload', max_load);
    $(find).attr('data-maxload', max_load);

    var $form = $(this), url = $form.attr('action'),
        formData = new FormData($form.get(0));

    sentVkForm(formData, url);
    that.parents('#modal-vkgroups').modal('hide');
});

$(".number-spinner .btn").on('click', function (e) {
    e.stopPropagation();

    btn = $(this);
    input = btn.closest('.number-spinner').find('input');
    btn.closest('.number-spinner').find('.btn').prop("disabled", false);

    if (btn.attr('data-dir') == 'up') {
        if (input.attr('max') == undefined || parseInt(input.val()) < parseInt(input.attr('max'))) {
            input.val(parseInt(input.val()) + 1);
        } else {
            btn.prop("disabled", true);
        }
    } else {
        if (input.attr('min') == undefined || parseInt(input.val()) > parseInt(input.attr('min'))) {
            input.val(parseInt(input.val()) - 1);
        } else {
            btn.prop("disabled", true);
        }
    }
});

$(".number-spinner .text-value").on('change', function (e) {
    e.stopPropagation();

    var input = $(this);
    if (input.attr('max') == undefined || parseInt(input.val()) > parseInt(input.attr('max'))) {
        input.val(input.attr('max'));
    } else if (input.attr('min') == undefined || parseInt(input.val()) < parseInt(input.attr('min'))) {
        input.val(input.attr('min'));
    }
});