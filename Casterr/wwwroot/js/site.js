// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var siteHub = new signalR.HubConnectionBuilder().withUrl("/hub").build();
async function start() {
    try {
        await siteHub.start();
        siteHub.invoke("Connected").catch(function (err) {
            return console.error(err.toString());
        });
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
}
siteHub.onclose(function () {
    if (typeof (onDisconnect) !== "undefined") { onDisconnect(); }
    start();
});

siteHub.on("ShowRenderer", (renderers) => {
    $("#renderers").empty();
    for (var i in renderers) {
        var castPlayer = $(Mustache.render($("#renderer_tmpl").html(), renderers[i]))
        updateCardRend(castPlayer, renderers[i].playback)
        $(castPlayer).find(".cast-pos").on('change', function (e) { CastSetPosition(renderers[i].id, e.target.value * 100); });
        $(castPlayer).find(".cast-vol").on('change', function (e) { CastSetVolume(renderers[i].id, e.target.value * 100); });

        $(castPlayer).find(".cast-backward").on('click', function (e) { CastControlPlayback(renderers[i].id, 4); });
        $(castPlayer).find(".cast-play").on('click', function (e) { CastControlPlayback(renderers[i].id, 2); });
        $(castPlayer).find(".cast-pause").on('click', function (e) { CastControlPlayback(renderers[i].id, 1); });
        $(castPlayer).find(".cast-stop").on('click', function (e) { CastControlPlayback(renderers[i].id, 3); });
        $(castPlayer).find(".cast-forward").on('click', function (e) { CastControlPlayback(renderers[i].id, 5); });

        $(castPlayer).find(".cast-mute").on('click', function (e) { CastControlPlayback(renderers[i].id, 6); });
        $(castPlayer).find(".cast-unmute").on('click', function (e) { CastControlPlayback(renderers[i].id, 7); });

        $(castPlayer).appendTo($("#renderers"));
    }
});
siteHub.on("updateRenderer", (renderer) => {
    updateCardRend($("#" + renderer.id), renderer.playback)
});
siteHub.on("updateRendererPos", (rendererpos) => {
    $("#" + rendererpos.id).find(".cast-pos").val(rendererpos.position);
});
function updateCardRend(elem, val) {
    $(elem).find(".cast-title").text(val.mediaTitle);

    $(elem).find(".cast-pos").val(val.position);
    $(elem).find(".cast-seek").text(val.seekable);
    if (val.mute) {
        $(elem).find(".cast-mute").hide();
        $(elem).find(".cast-unmute").show();
    } else {
        $(elem).find(".cast-mute").show();
        $(elem).find(".cast-unmute").hide();
    }
    switch (val.status) {
        case 0:
            $(elem).find(".cast-sub").text("Opening");
            break;
        case 1:
            $(elem).find(".cast-sub").text("Stop");
            break;
        case 2:
            $(elem).find(".cast-sub").text("Pause");
            break;
        case 3:
            $(elem).find(".cast-sub").text("Buffering");
            break;
        case 4:
            $(elem).find(".cast-sub").text("Playing");
            break;
        case 5:
            $(elem).find(".cast-sub").text("Error");
            break;
        case 6:
            $(elem).find(".cast-sub").text("Corked");
            break;
    }
    $(elem).find(".tracks").html($(Mustache.render($("#renderer_tracks_tmpl").html(), { audio: val.audioTracks, sub: val.subTracks })));
    $(elem).find(".audioTrackSelector option[value=" + val.audioTrack + "]").attr('selected', true);
    $(elem).find(".subTrackSelector option[value=" + val.subTrack + "]").attr('selected', true);

}
function CastSetVolume(id, vol) {
    siteHub.invoke("SetVolume", id, vol).catch(function (err) { return console.error(err.toString()); });
}
function CastSetPosition(id, pos) {
    siteHub.invoke("SetPosition", id, pos).catch(function (err) { return console.error(err.toString()); });
}
function CastControlPlayback(id, code) {
    siteHub.invoke("ControlPlayback", id, code).catch(function (err) { return console.error(err.toString()); });
}
function setAudio(elem) {
    siteHub.invoke("setTrack", $(elem).parents(".card").attr('id'), true, parseInt($(elem).val())).catch(function (err) { return console.error(err.toString()); });
}
function setSub(elem) {
    siteHub.invoke("setTrack", $(elem).parents(".card").attr('id'), false, parseInt($(elem).val())).catch(function (err) { return console.error(err.toString()); });
}

$(function () {
    if (userAuthorized) {
        start();
    }
});