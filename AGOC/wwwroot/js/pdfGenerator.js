function loadFont(url, callback) {
    const xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.responseType = 'arraybuffer';
    xhr.onload = function () {
        if (this.status === 200) {
            const font = btoa(
                new Uint8Array(this.response)
                    .reduce((data, byte) => data + String.fromCharCode(byte), '')
            );
            callback(font);
        }
    };
    xhr.send();
}

function getCurrentFormattedDate() {
    const date = new Date();
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

async function generatePDF({ fontUrl, imgUrl, pdfTitle, pdfSubject, pdfAuthor, pdfKeywords, pdfCreator, headerText, tableHeaders, tableData, engagementsTitle, engagementsData }) {
    loadFont(fontUrl, function (font) {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF({
            orientation: 'portrait',
            unit: 'mm',
            format: 'a4',
            putOnlyUsedFonts: true
        });

        // Add the custom font
        doc.addFileToVFS("CustomFont.ttf", font);
        doc.addFont("CustomFont.ttf", "CustomFont", "normal");
        doc.setFont("CustomFont");

        // Optional: Set some metadata
        doc.setProperties({
            title: pdfTitle,
            subject: pdfSubject,
            author: pdfAuthor,
            keywords: pdfKeywords,
            creator: pdfCreator
        });

        // Get the current date
        const currentDate = getCurrentFormattedDate();

        // Add the image to the PDF
        const img = new Image();
        img.src = imgUrl;
        img.onload = function () {
            const imgWidth = 200; // Image width in mm
            const imgHeight = (img.height / img.width) * imgWidth; // Maintain aspect ratio
            const x = (doc.internal.pageSize.getWidth() - imgWidth) / 2; // Center the image
            const y = 10; // Top margin

            doc.addImage(img, 'PNG', x, y, imgWidth, imgHeight);

            // Add the header text and align it to the center
            const text = `${headerText} ${currentDate}`;
            const pageWidth = doc.internal.pageSize.getWidth();
            const headerY = y + imgHeight + 10; // Below the image
            doc.text(text, pageWidth / 2, headerY, { align: 'center' });

            // Add the appointments table
            doc.autoTable({
                startY: headerY + 10, // Start the table below the header text
                head: [tableHeaders],
                body: tableData,
                styles: {
                    font: "CustomFont",
                    fillColor: [255, 255, 255],
                    cellPadding: 3,
                    halign: 'center'
                },
                headStyles: {
                    textColor: [0, 0, 0],
                    fillColor: [211, 211, 211],
                    halign: 'center'
                },
                margin: { right: 15, left: 15 },
                theme: 'grid',
                rtl: true
            });

            // Add the text above the engagements table
            doc.text(engagementsTitle, pageWidth / 2, doc.previousAutoTable.finalY + 10, { align: 'center' });

            // Add the engagements table
            doc.autoTable({
                startY: doc.previousAutoTable.finalY + 20, // Start the table below the title text
                head: [tableHeaders],
                body: engagementsData,
                styles: {
                    font: "CustomFont",
                    fillColor: [255, 255, 255],
                    cellPadding: 3,
                    halign: 'center'
                },
                headStyles: {
                    textColor: [0, 0, 0],
                    fillColor: [211, 211, 211],
                    halign: 'center'
                },
                margin: { right: 14, left: 14 },
                theme: 'grid',
                rtl: true
            });

            // Create a Blob from the PDF
            const pdfOutput = doc.output('blob');
            const blobUrl = URL.createObjectURL(pdfOutput);

            // Open the Blob in a new tab
            window.open(blobUrl);
        };
    });
}
