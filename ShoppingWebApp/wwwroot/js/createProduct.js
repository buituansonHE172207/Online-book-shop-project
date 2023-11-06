function upLoadImage() {
    const ref = firebase.storage().ref();
    const file = document.querySelector("#image").files[0];
    if (file != null) {
        const metadata = {
            contentType: file.type
        };

        const name = file.name;
        const uploadIMG = ref.child(name).put(file, metadata);
        uploadIMG
            .then(snapshot => snapshot.ref.getDownloadURL())
            .then(url => {
                $("#uploadedImg").val(url);
                $("#productForm").submit();
            })
            .catch(console.error)
    }
}