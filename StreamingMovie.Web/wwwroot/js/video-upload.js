console.log("✅ movie-upload.js loaded");

class MovieUploadManager {
    constructor() {
        this.selectedMovieFile = null;
        this.selectedPosterFile = null;
        this.selectedSubtitleFile = null;
        this.chunkSize = 2 * 1024 * 1024; // 2MB chunks for video files
        this.currentUploads = new Map();

        // Wait for DOM to be ready
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => {
                this.init();
            });
        } else {
            this.init();
        }
    }

    init() {
        console.log('MovieUploadManager initializing...');
        this.initializeElements();
        this.setupEventListeners();
        this.setupFormValidation();
        this.loadMoviesList();
        console.log('MovieUploadManager initialized successfully');
    }

    initializeElements() {
        // File upload areas
        this.posterUploadArea = document.querySelector('input[accept="image/*"]')?.parentElement;
        this.movieUploadArea = document.querySelector('input[accept="video/*"]')?.parentElement;
        this.subtitleUploadArea = document.querySelector('input[accept=".srt"]')?.parentElement;

        // File inputs
        this.posterInput = document.querySelector('input[accept="image/*"]');
        this.movieInput = document.querySelector('input[accept="video/*"]');
        this.subtitleInput = document.querySelector('input[accept=".srt"]');

        // Form elements
        this.form = document.querySelector('form[asp-controller="Admin"]');
        this.submitBtn = document.querySelector('button[type="submit"]');
        this.cancelBtn = document.querySelector('button[type="button"]');

        // Search and filter
        this.searchInput = document.querySelector('input[placeholder="Search movies..."]');
        this.statusFilter = document.querySelector('select option[value="published"]')?.parentElement;

        // Movies table
        this.moviesTable = document.querySelector('tbody');

        console.log('Elements initialized:', {
            posterUploadArea: !!this.posterUploadArea,
            movieUploadArea: !!this.movieUploadArea,
            subtitleUploadArea: !!this.subtitleUploadArea,
            form: !!this.form,
            submitBtn: !!this.submitBtn,
            moviesTable: !!this.moviesTable
        });
    }

    setupEventListeners() {
        // File upload handlers
        this.setupFileUploadHandlers();

        // Form handlers
        this.setupFormHandlers();

        // Search and filter handlers
        this.setupSearchAndFilterHandlers();

        // Table action handlers
        this.setupTableActionHandlers();
    }

    setupFileUploadHandlers() {
        // Poster upload
        if (this.posterInput && this.posterUploadArea) {
            this.posterInput.addEventListener('change', (e) => {
                this.handleFileSelect(e.target.files[0], 'poster');
            });

            this.setupDragAndDrop(this.posterUploadArea, this.posterInput, 'image/*', 'poster');
        }

        // Movie file upload
        if (this.movieInput && this.movieUploadArea) {
            this.movieInput.addEventListener('change', (e) => {
                this.handleFileSelect(e.target.files[0], 'movie');
            });

            this.setupDragAndDrop(this.movieUploadArea, this.movieInput, 'video/*', 'movie');
        }

        // Subtitle upload
        if (this.subtitleInput && this.subtitleUploadArea) {
            this.subtitleInput.addEventListener('change', (e) => {
                this.handleFileSelect(e.target.files[0], 'subtitle');
            });

            this.setupDragAndDrop(this.subtitleUploadArea, this.subtitleInput, '.srt', 'subtitle');
        }
    }

    setupDragAndDrop(uploadArea, fileInput, acceptType, fileType) {
        // Click to select
        uploadArea.addEventListener('click', (e) => {
            e.preventDefault();
            fileInput.click();
        });

        // Drag over
        uploadArea.addEventListener('dragover', (e) => {
            e.preventDefault();
            uploadArea.classList.add('border-blue-500', 'bg-blue-50');
        });

        // Drag leave
        uploadArea.addEventListener('dragleave', () => {
            uploadArea.classList.remove('border-blue-500', 'bg-blue-50');
        });

        // Drop
        uploadArea.addEventListener('drop', (e) => {
            e.preventDefault();
            uploadArea.classList.remove('border-blue-500', 'bg-blue-50');

            const files = e.dataTransfer.files;
            if (files.length > 0) {
                if (this.validateFileType(files[0], acceptType)) {
                    this.handleFileSelect(files[0], fileType);
                }
            }
        });
    }

    validateFileType(file, acceptType) {
        if (acceptType === 'image/*') {
            return file.type.startsWith('image/');
        } else if (acceptType === 'video/*') {
            return file.type.startsWith('video/') || this.isVideoFile(file.name);
        } else if (acceptType === '.srt') {
            return file.name.toLowerCase().endsWith('.srt');
        }
        return false;
    }

    isVideoFile(filename) {
        const videoExtensions = ['.mp4', '.webm', '.ogg', '.avi', '.mov', '.wmv', '.flv', '.mkv', '.m4v'];
        const extension = filename.toLowerCase().substr(filename.lastIndexOf('.'));
        return videoExtensions.includes(extension);
    }

    handleFileSelect(file, fileType) {
        if (!file) return;

        console.log(`${fileType} file selected:`, file.name, file.type, file.size);

        // Store the file
        switch (fileType) {
            case 'poster':
                this.selectedPosterFile = file;
                this.updateUploadArea(this.posterUploadArea, file, 'poster');
                break;
            case 'movie':
                this.selectedMovieFile = file;
                this.updateUploadArea(this.movieUploadArea, file, 'movie');
                break;
            case 'subtitle':
                this.selectedSubtitleFile = file;
                this.updateUploadArea(this.subtitleUploadArea, file, 'subtitle');
                break;
        }

        this.validateForm();
    }

    updateUploadArea(uploadArea, file, fileType) {
        const iconClass = fileType === 'poster' ? 'fa-image' :
            fileType === 'movie' ? 'fa-film' : 'fa-file-text';

        const content = uploadArea.querySelector('.flex.flex-col');
        content.innerHTML = `
            <i class="fa ${iconClass} text-2xl text-green-500 mb-2"></i>
            <p class="text-sm text-green-600 font-medium">${file.name}</p>
            <p class="text-xs text-gray-500">${this.formatFileSize(file.size)}</p>
            <button type="button" class="mt-2 text-xs text-red-500 hover:text-red-700" onclick="movieUploader.removeFile('${fileType}')">
                Remove
            </button>
        `;
    }

    removeFile(fileType) {
        switch (fileType) {
            case 'poster':
                this.selectedPosterFile = null;
                this.resetUploadArea(this.posterUploadArea, 'poster', 'fa-cloud-upload-alt', 'Click to upload poster');
                break;
            case 'movie':
                this.selectedMovieFile = null;
                this.resetUploadArea(this.movieUploadArea, 'movie', 'fa-film', 'Click to upload movie file');
                break;
            case 'subtitle':
                this.selectedSubtitleFile = null;
                this.resetUploadArea(this.subtitleUploadArea, 'subtitle', 'fa-file-text', 'Upload subtitle (.srt)');
                break;
        }
        this.validateForm();
    }

    resetUploadArea(uploadArea, fileType, iconClass, text) {
        const content = uploadArea.querySelector('.flex.flex-col');
        content.innerHTML = `
            <i class="fa ${iconClass} text-3xl text-gray-400 mb-2"></i>
            <p class="text-sm text-gray-500 dark:text-gray-400">${text}</p>
        `;
    }

    setupFormHandlers() {
        if (this.form) {
            this.form.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleFormSubmit();
            });
        }

        if (this.cancelBtn) {
            this.cancelBtn.addEventListener('click', () => {
                this.resetForm();
            });
        }
    }

    async handleFormSubmit() {
        if (!this.validateForm()) {
            this.showMessage('Please fill in all required fields', 'error');
            return;
        }

        this.submitBtn.disabled = true;
        this.submitBtn.innerHTML = '<i class="fa fa-spinner fa-spin mr-2"></i>Uploading...';

        try {
            // Create progress overlay
            this.showUploadProgress();

            const formData = this.collectFormData();

            // Upload files first if they exist
            if (this.selectedMovieFile) {
                const movieUrl = await this.uploadLargeFile(this.selectedMovieFile, 'movie');
                formData.append('MovieFileUrl', movieUrl);
            }

            if (this.selectedPosterFile) {
                const posterUrl = await this.uploadFile(this.selectedPosterFile, 'poster');
                formData.append('PosterUrl', posterUrl);
            }

            if (this.selectedSubtitleFile) {
                const subtitleUrl = await this.uploadFile(this.selectedSubtitleFile, 'subtitle');
                formData.append('SubtitleUrl', subtitleUrl);
            }

            // Submit the form
            const response = await fetch('/Admin/Upload', {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                this.showMessage('Movie uploaded successfully!', 'success');
                this.resetForm();
                this.loadMoviesList();
            } else {
                throw new Error('Upload failed');
            }

        } catch (error) {
            console.error('Upload error:', error);
            this.showMessage('Upload failed: ' + error.message, 'error');
        } finally {
            this.submitBtn.disabled = false;
            this.submitBtn.innerHTML = '<i class="fa fa-upload mr-2"></i>Upload Movie';
            this.hideUploadProgress();
        }
    }

    collectFormData() {
        const formData = new FormData();

        // Get all form inputs
        const inputs = this.form.querySelectorAll('input, select, textarea');
        inputs.forEach(input => {
            if (input.type === 'file') return; // Skip file inputs
            if (input.type === 'checkbox') {
                formData.append(input.name || input.id, input.checked);
            } else if (input.value) {
                formData.append(input.name || input.id, input.value);
            }
        });

        return formData;
    }

    async uploadLargeFile(file, fileType) {
        const totalChunks = Math.ceil(file.size / this.chunkSize);
        const chunkId = this.generateChunkId();

        for (let chunkNumber = 0; chunkNumber < totalChunks; chunkNumber++) {
            const start = chunkNumber * this.chunkSize;
            const end = Math.min(start + this.chunkSize, file.size);
            const chunk = file.slice(start, end);

            await this.uploadChunk(chunk, chunkNumber, totalChunks, chunkId, file.name);

            const progress = ((chunkNumber + 1) / totalChunks) * 100;
            this.updateUploadProgress(progress, `Uploading ${fileType}...`);
        }

        return `/uploads/${fileType}s/${chunkId}/${file.name}`;
    }

    async uploadFile(file, fileType) {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('fileType', fileType);

        const response = await fetch('/Admin/UploadFile', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error(`Failed to upload ${fileType}`);
        }

        const result = await response.json();
        return result.url;
    }

    async uploadChunk(chunk, chunkNumber, totalChunks, chunkId, fileName) {
        const formData = new FormData();
        formData.append('ChunkFile', chunk);
        formData.append('FileName', fileName);
        formData.append('ChunkNumber', chunkNumber.toString());
        formData.append('TotalChunks', totalChunks.toString());
        formData.append('ChunkId', chunkId);

        const response = await fetch('/Admin/UploadChunk', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error(`Chunk upload failed: ${response.status}`);
        }

        const result = await response.json();
        if (!result.success) {
            throw new Error(result.message);
        }

        return result;
    }

    generateChunkId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }

    showUploadProgress() {
        const overlay = document.createElement('div');
        overlay.id = 'uploadOverlay';
        overlay.className = 'fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50';
        overlay.innerHTML = `
            <div class="bg-white dark:bg-slate-800 rounded-lg p-6 max-w-md w-full mx-4">
                <h3 class="text-lg font-semibold mb-4 text-slate-700 dark:text-white">Uploading Movie...</h3>
                <div class="w-full bg-gray-200 rounded-full h-2.5 dark:bg-gray-700">
                    <div id="uploadProgressBar" class="bg-blue-600 h-2.5 rounded-full" style="width: 0%"></div>
                </div>
                <p id="uploadProgressText" class="text-sm text-gray-500 mt-2">0%</p>
            </div>
        `;
        document.body.appendChild(overlay);
    }

    updateUploadProgress(percentage, text) {
        const progressBar = document.getElementById('uploadProgressBar');
        const progressText = document.getElementById('uploadProgressText');

        if (progressBar && progressText) {
            progressBar.style.width = percentage + '%';
            progressText.textContent = `${Math.round(percentage)}% - ${text}`;
        }
    }

    hideUploadProgress() {
        const overlay = document.getElementById('uploadOverlay');
        if (overlay) {
            overlay.remove();
        }
    }

    setupFormValidation() {
        const requiredFields = this.form.querySelectorAll('input[required], select[required]');
        requiredFields.forEach(field => {
            field.addEventListener('input', () => this.validateForm());
            field.addEventListener('change', () => this.validateForm());
        });
    }

    validateForm() {
        const title = this.form.querySelector('input[placeholder="Enter movie title"]');
        const genre = this.form.querySelector('select option[value="action"]')?.parentElement;
        const country = this.form.querySelector('select option[value="1"]')?.parentElement;
        const releaseYear = this.form.querySelector('input[placeholder="2024"]');
        const duration = this.form.querySelector('input[placeholder="120"]');
        const quality = this.form.querySelector('select option[value="480p"]')?.parentElement;

        const isValid = title?.value?.trim() &&
            genre?.value &&
            country?.value &&
            releaseYear?.value &&
            duration?.value &&
            quality?.value &&
            this.selectedMovieFile;

        if (this.submitBtn) {
            this.submitBtn.disabled = !isValid;
        }

        return isValid;
    }

    resetForm() {
        this.form.reset();
        this.selectedMovieFile = null;
        this.selectedPosterFile = null;
        this.selectedSubtitleFile = null;

        // Reset upload areas
        this.resetUploadArea(this.posterUploadArea, 'poster', 'fa-cloud-upload-alt', 'Click to upload poster');
        this.resetUploadArea(this.movieUploadArea, 'movie', 'fa-film', 'Click to upload movie file');
        this.resetUploadArea(this.subtitleUploadArea, 'subtitle', 'fa-file-text', 'Upload subtitle (.srt)');

        this.validateForm();
    }

    setupSearchAndFilterHandlers() {
        if (this.searchInput) {
            this.searchInput.addEventListener('input', (e) => {
                this.filterMovies(e.target.value, this.statusFilter?.value);
            });
        }

        if (this.statusFilter) {
            this.statusFilter.addEventListener('change', (e) => {
                this.filterMovies(this.searchInput?.value, e.target.value);
            });
        }
    }

    filterMovies(searchTerm, statusFilter) {
        const rows = this.moviesTable.querySelectorAll('tr');

        rows.forEach(row => {
            const title = row.querySelector('h6')?.textContent.toLowerCase() || '';
            const status = row.querySelector('span')?.textContent.toLowerCase() || '';

            const matchesSearch = !searchTerm || title.includes(searchTerm.toLowerCase());
            const matchesStatus = !statusFilter || status.includes(statusFilter.toLowerCase());

            row.style.display = matchesSearch && matchesStatus ? '' : 'none';
        });
    }

    setupTableActionHandlers() {
        if (this.moviesTable) {
            this.moviesTable.addEventListener('click', (e) => {
                if (e.target.matches('.fa-edit') || e.target.matches('[title="Edit"]')) {
                    const row = e.target.closest('tr');
                    this.editMovie(row);
                } else if (e.target.matches('.fa-eye') || e.target.matches('[title="View"]')) {
                    const row = e.target.closest('tr');
                    this.viewMovie(row);
                } else if (e.target.matches('.fa-trash') || e.target.matches('[title="Delete"]')) {
                    const row = e.target.closest('tr');
                    this.deleteMovie(row);
                }
            });
        }
    }

    editMovie(row) {
        const title = row.querySelector('h6')?.textContent;
        console.log('Edit movie:', title);
        // Implement edit functionality
    }

    viewMovie(row) {
        const title = row.querySelector('h6')?.textContent;
        console.log('View movie:', title);
        // Implement view functionality
    }

    async deleteMovie(row) {
        const title = row.querySelector('h6')?.textContent;

        if (confirm(`Are you sure you want to delete "${title}"?`)) {
            try {
                // Get movie ID from row data
                const movieId = row.dataset.movieId;

                const response = await fetch(`/Admin/DeleteMovie/${movieId}`, {
                    method: 'DELETE'
                });

                if (response.ok) {
                    row.remove();
                    this.showMessage('Movie deleted successfully', 'success');
                } else {
                    throw new Error('Failed to delete movie');
                }
            } catch (error) {
                console.error('Delete error:', error);
                this.showMessage('Failed to delete movie', 'error');
            }
        }
    }

    async loadMoviesList() {
        try {
            const response = await fetch('/Admin/GetMovies');
            const movies = await response.json();

            // Update the table with new data
            // This would typically be handled by server-side rendering
            console.log('Movies loaded:', movies.length);

        } catch (error) {
            console.error('Error loading movies:', error);
        }
    }

    formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    showMessage(message, type) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `fixed top-4 right-4 z-50 px-4 py-3 rounded-lg ${type === 'success' ? 'bg-green-500 text-white' : 'bg-red-500 text-white'
            }`;
        messageDiv.textContent = message;

        document.body.appendChild(messageDiv);

        setTimeout(() => {
            messageDiv.remove();
        }, 5000);
    }
}

// Initialize the movie uploader when the page loads
let movieUploader;
document.addEventListener('DOMContentLoaded', () => {
    console.log('DOM loaded, initializing MovieUploadManager');
    movieUploader = new MovieUploadManager();
});

// Make it globally available for onclick handlers
window.movieUploader = movieUploader;