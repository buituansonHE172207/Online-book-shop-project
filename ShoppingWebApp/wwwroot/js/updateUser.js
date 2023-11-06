document.addEventListener("DOMContentLoaded", function () {
    $("#userAvatar").click(function () {
        $(this).val("");
    });

    $("#userAvatar").change(function () {

        /*        var img = $("#userAvatar").val();*/
        if ($("#userAvatar").get(0).files.length !== 0) {
            $("#avatarLoading").removeAttr("hidden");
            // Upload Image
            const ref = firebase.storage().ref();
            const file = $("#userAvatar").prop('files')[0];
            const metadata = {
                contentType: "img/png"
            };

            const name = "UserAvatar_" + $("#UserId").val() + ".png";
            //const name = "UserAvatar_Default.png";

            const uploadIMG = ref.child(name).put(file, metadata);
            uploadIMG
                .then(snapshot => snapshot.ref.getDownloadURL())
                .then(url => {
                    $("#Avatar").val(url);
                    $("#avatarImg").attr("src", url);
                    $("#avatarLoading").html("Uploaded successfully!!");
                })
                .catch(console.error);
        }
    });
});