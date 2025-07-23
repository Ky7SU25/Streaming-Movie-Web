console.log("✅ video-upload.js loaded");

class VideoUploadManager {
    constructor() {
        this.currentTab = 'movie';
        this.uploadProgress = {};
        this.chunkSize = 5 * 1024 * 1024; // 5MB chunks for MinIO
        this.concurrency = 4; // Number of concurrent part uploads
        this.minioConfig = {
            // MinIO upload endpoints
            startEndpoint: '/Admin/Upload/Start',
            completeEndpoint: '/Admin/Upload/Complete',
            abortEndpoint: '/Admin/Upload/Abort'
        };
        
        this.init();
    }

    init() {
        console.log('🚀 Initializing VideoUploadManager');
        this.debugDOMElements();
        this.setupTabs();
        this.setupForms();
        this.setupFileUploads();
        this.setupSlugGeneration();
        this.loadSeriesList(); // Load series for episode dropdown
        console.log('✅ VideoUploadManager initialized successfully');
    }

    debugDOMElements() {
        console.log('🔍 Debugging DOM elements...');
        
        // Check file inputs
        const fileInputs = document.querySelectorAll('input[type="file"]');
        console.log(`📁 File inputs found: ${fileInputs.length}`);
        fileInputs.forEach(input => {
            console.log(`  - ${input.id}: ${input.accept}`);
        });
        
        // Check preview containers
        const previewContainers = document.querySelectorAll('[id$="-preview"]');
        console.log(`👁️ Preview containers found: ${previewContainers.length}`);
        previewContainers.forEach(container => {
            console.log(`  - ${container.id}: ${container.classList.contains('hidden') ? 'hidden' : 'visible'}`);
        });
        
        // Check video elements
        const videoElements = document.querySelectorAll('video');
        console.log(`🎥 Video elements found: ${videoElements.length}`);
        videoElements.forEach(video => {
            console.log(`  - ${video.id}`);
        });
        
        // Check image elements
        const imageElements = document.querySelectorAll('img[id$="-image"]');
        console.log(`🖼️ Image elements found: ${imageElements.length}`);
        imageElements.forEach(img => {
            console.log(`  - ${img.id}`);
        });
    }

    setupTabs() {
        const tabButtons = document.querySelectorAll('.tab-button');
        console.log('Tab buttons found:', tabButtons.length);
        tabButtons.forEach(button => {
            button.addEventListener('click', (e) => {
                e.preventDefault();
                e.stopPropagation();
                
                const tabId = button.id.replace('-tab', '');
                console.log('Tab clicked:', tabId);
                
                this.switchTab(tabId);
            });
        });
    }

    switchTab(tabId) {
        console.log('Switching to tab:', tabId);
        
        // Remove active class from all tabs
        document.querySelectorAll('.tab-button').forEach(btn => {
            btn.classList.remove('border-blue-500', 'text-blue-600');
            btn.classList.add('border-transparent', 'text-gray-500');
        });
        
        // Hide all tab contents
        document.querySelectorAll('.tab-content').forEach(content => {
            content.classList.add('hidden');
        });

        // Activate selected tab button
        const activeButton = document.getElementById(`${tabId}-tab`);
        if (activeButton) {
            activeButton.classList.remove('border-transparent', 'text-gray-500');
            activeButton.classList.add('border-blue-500', 'text-blue-600');
        }
        
        // Show selected tab content
        const activeContent = document.getElementById(`${tabId}-form`);
        if (activeContent) {
            activeContent.classList.remove('hidden');
            this.currentTab = tabId;
            console.log('Tab switched successfully to:', tabId);
        } else {
            console.error('Tab content not found for:', tabId);
        }
    }

    setupForms() {
        // Movie Form
        const movieForm = document.getElementById('movieUploadForm');
        if (movieForm) {
            movieForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleMovieUpload(new FormData(movieForm));
            });
        }

        // Series Form
        const seriesForm = document.getElementById('seriesUploadForm');
        if (seriesForm) {
            seriesForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleSeriesUpload(new FormData(seriesForm));
            });
        }

        // Episode Form
        const episodeForm = document.getElementById('episodeUploadForm');
        if (episodeForm) {
            episodeForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleEpisodeUpload(new FormData(episodeForm));
            });
        }
    }

    setupFileUploads() {
        // File input change handlers
        const fileInputs = document.querySelectorAll('input[type="file"]');
        console.log(`📁 Found ${fileInputs.length} file inputs`);
        
        fileInputs.forEach(input => {
            input.addEventListener('change', (e) => {
                console.log(`📁 File input changed: ${input.id}`);
                this.handleFileSelect(e.target);
            });
        });

        // Drag and drop handlers
        const dropZones = document.querySelectorAll('.border-dashed');
        console.log(`📦 Found ${dropZones.length} drop zones`);
        
        dropZones.forEach(zone => {
            zone.addEventListener('dragover', (e) => this.handleDragOver(e));
            zone.addEventListener('dragleave', (e) => this.handleDragLeave(e));
            zone.addEventListener('drop', (e) => this.handleDrop(e, zone));
        });
    }

    handleFileSelect(input) {
        const file = input.files[0];
        if (!file) {
            console.log('❌ No file selected');
            return;
        }

        console.log(`📁 Selected file: ${file.name} (${this.formatFileSize(file.size)}) - Type: ${file.type}`);
        
        // Get file type from input ID (format: {formType}-{fileType})
        const inputId = input.id;
        const parts = inputId.split('-');
        if (parts.length < 2) {
            console.error('❌ Invalid input ID format:', inputId);
            return;
        }
        
        const formType = parts[0]; // movie, series, episode
        const fileType = parts[1]; // video, poster, banner, subtitle
        
        console.log(`📂 Detected: formType=${formType}, fileType=${fileType}`);
        
        // Show appropriate preview based on file type
        if (file.type.startsWith('image/') || this.isImageFile(file.name)) {
            console.log('🖼️ Showing image preview');
            this.showImagePreview(file, formType, fileType);
        } else if (file.type.startsWith('video/') || this.isVideoFile(file.name)) {
            console.log('🎥 Showing video preview');
            this.showVideoPreview(file, formType, fileType);
        } else {
            console.log('📄 File type not recognized for preview:', file.type);
            // For other files like subtitles, just show filename
            this.showFileInfo(file, formType, fileType);
        }
    }

    showImagePreview(file, formType, fileType) {
        const previewContainer = document.getElementById(`${formType}-${fileType}-preview`);
        const imageElement = document.getElementById(`${formType}-${fileType}-image`);
        const filenameElement = document.getElementById(`${formType}-${fileType}-filename`);
        
        console.log(`🖼️ Preview elements:`, {
            previewContainer: !!previewContainer,
            imageElement: !!imageElement,
            filenameElement: !!filenameElement
        });
        
        if (previewContainer && imageElement) {
            const reader = new FileReader();
            reader.onload = (e) => {
                imageElement.src = e.target.result;
                if (filenameElement) filenameElement.textContent = file.name;
                previewContainer.classList.remove('hidden');
                console.log('✅ Image preview shown successfully');
            };
            reader.onerror = (e) => {
                console.error('❌ FileReader error:', e);
            };
            reader.readAsDataURL(file);
        } else {
            console.error('❌ Image preview elements not found:', {
                previewId: `${formType}-${fileType}-preview`,
                imageId: `${formType}-${fileType}-image`
            });
        }
    }

    showVideoPreview(file, formType, fileType) {
        const previewContainer = document.getElementById(`${formType}-${fileType}-preview`);
        const videoElement = document.getElementById(`${formType}-${fileType}-player`);
        const processingOverlay = document.getElementById(`${formType}-${fileType}-processing`);
        const filenameElement = document.getElementById(`${formType}-${fileType}-filename`);
        
        console.log(`🎥 Video preview elements:`, {
            previewContainer: !!previewContainer,
            videoElement: !!videoElement,
            processingOverlay: !!processingOverlay,
            filenameElement: !!filenameElement
        });
        
        if (previewContainer && videoElement) {
            // Check file size limits for preview
            const maxPreviewSize = 500 * 1024 * 1024; // 500MB limit for preview
            const isLargeFile = file.size > maxPreviewSize;
            
            // Show preview container with processing overlay
            previewContainer.classList.remove('hidden');
            if (processingOverlay) processingOverlay.classList.remove('hidden');
            if (filenameElement) filenameElement.textContent = file.name;
            
            console.log(`🎥 Video file size: ${this.formatFileSize(file.size)}, isLarge: ${isLargeFile}`);
            
            if (isLargeFile) {
                // For large files, show info instead of preview
                console.log('📊 Showing large file info instead of preview');
                setTimeout(() => {
                    if (processingOverlay) {
                        processingOverlay.innerHTML = `
                            <div class="text-center text-white">
                                <i class="fas fa-video text-4xl mb-4 text-blue-400"></i>
                                <p class="text-lg font-semibold">Large Video File</p>
                                <p class="text-sm text-gray-300 mb-2">${file.name}</p>
                                <p class="text-sm text-gray-300 mb-4">Size: ${this.formatFileSize(file.size)}</p>
                                <p class="text-xs text-gray-400">Preview not available for files larger than 500MB</p>
                                <p class="text-xs text-gray-400">Video will be processed during upload</p>
                                <div class="mt-4 flex justify-center">
                                    <div class="flex items-center space-x-2 text-green-400">
                                        <i class="fas fa-check-circle"></i>
                                        <span class="text-sm">File ready for upload</span>
                                    </div>
                                </div>
                            </div>
                        `;
                    }
                }, 1000);
                return;
            }
            
            // For smaller files, create blob URL with safety checks
            try {
                const videoUrl = URL.createObjectURL(file);
                console.log('🎥 Created video URL:', videoUrl);
                
                // Simulate processing time (2-3 seconds)
                setTimeout(() => {
                    videoElement.src = videoUrl;
                    if (processingOverlay) processingOverlay.classList.add('hidden');
                    
                    videoElement.addEventListener('loadeddata', () => {
                        console.log('✅ Video loaded successfully');
                    });
                    
                    videoElement.addEventListener('error', (e) => {
                        console.error('❌ Video load error:', e);
                        if (processingOverlay) {
                            processingOverlay.classList.remove('hidden');
                            processingOverlay.innerHTML = `
                                <div class="text-center text-white">
                                    <i class="fas fa-exclamation-triangle text-4xl mb-4 text-red-500"></i>
                                    <p>Unable to preview video</p>
                                    <p class="text-sm text-gray-300">${file.name}</p>
                                    <div class="mt-4 flex justify-center">
                                        <div class="flex items-center space-x-2 text-green-400">
                                            <i class="fas fa-upload"></i>
                                            <span class="text-sm">File ready for upload</span>
                                        </div>
                                    </div>
                                </div>
                            `;
                        }
                    });
                }, 2000 + Math.random() * 1000);
            } catch (error) {
                console.error('❌ Video preview error:', error);
                if (processingOverlay) {
                    processingOverlay.innerHTML = `
                        <div class="text-center text-white">
                            <i class="fas fa-exclamation-triangle text-4xl mb-4 text-yellow-500"></i>
                            <p class="text-lg font-semibold">Preview Unavailable</p>
                            <p class="text-sm text-gray-300 mb-2">${file.name}</p>
                            <p class="text-sm text-gray-300 mb-4">Size: ${this.formatFileSize(file.size)}</p>
                            <p class="text-xs text-gray-400 mb-2">${error.message}</p>
                            <div class="mt-4 flex justify-center">
                                <div class="flex items-center space-x-2 text-green-400">
                                    <i class="fas fa-upload"></i>
                                    <span class="text-sm">File ready for upload</span>
                                </div>
                            </div>
                        </div>
                    `;
                }
            }
        } else {
            console.error('❌ Video preview elements not found:', {
                previewId: `${formType}-${fileType}-preview`,
                videoId: `${formType}-${fileType}-player`
            });
        }
    }

    showFileInfo(file, formType, fileType) {
        // For non-preview files like subtitles, just show the filename
        const filenameElement = document.getElementById(`${formType}-${fileType}-filename`);
        if (filenameElement) {
            filenameElement.textContent = file.name;
        }
        console.log(`📄 File info shown for: ${file.name}`);
    }

    isImageFile(filename) {
        const imageExtensions = ['.jpg', '.jpeg', '.png', '.gif', '.webp', '.bmp', '.svg'];
        const extension = filename.toLowerCase().substr(filename.lastIndexOf('.'));
        return imageExtensions.includes(extension);
    }

    isVideoFile(filename) {
        const videoExtensions = ['.mp4', '.webm', '.ogg', '.avi', '.mov', '.wmv', '.flv', '.mkv', '.m4v'];
        const extension = filename.toLowerCase().substr(filename.lastIndexOf('.'));
        return videoExtensions.includes(extension);
    }

    handleDragOver(e) {
        e.preventDefault();
        e.currentTarget.classList.add('border-blue-500', 'bg-blue-50');
    }

    handleDragLeave(e) {
        e.preventDefault();
        e.currentTarget.classList.remove('border-blue-500', 'bg-blue-50');
    }

    handleDrop(e, dropZone) {
        e.preventDefault();
        dropZone.classList.remove('border-blue-500', 'bg-blue-50');
        
        const files = e.dataTransfer.files;
        if (files.length > 0) {
            const fileInput = dropZone.querySelector('input[type="file"]');
            if (fileInput) {
                // Transfer files to input
                fileInput.files = files;
                console.log(`📦 Dropped file: ${files[0].name}`);
                this.handleFileSelect(fileInput);
            }
        }
    }

    setupSlugGeneration() {
        const titleInputs = ['movie-title', 'series-title'];
        const slugInputs = ['movie-slug', 'series-slug'];

        titleInputs.forEach((titleId, index) => {
            const titleInput = document.getElementById(titleId);
            const slugInput = document.getElementById(slugInputs[index]);
            
            if (titleInput && slugInput) {
                titleInput.addEventListener('input', (e) => {
                    const slug = this.generateSlug(e.target.value);
                    slugInput.value = slug;
                    this.validateSlug(slug, index === 0 ? 'movie' : 'series');
                });
            }
        });
    }

    async loadSeriesList() {
        try {
            const response = await fetch('/Admin/GetSeriesList');
            if (response.ok) {
                const result = await response.json();
                if (result.success && result.data) {
                    const seriesSelect = document.querySelector('select[name="seriesId"]');
                    if (seriesSelect) {
                        seriesSelect.innerHTML = '<option value="">Choose a series</option>';
                        
                        result.data.forEach(series => {
                            const option = document.createElement('option');
                            option.value = series.id;
                            option.textContent = `${series.title} (${series.totalSeasons} season${series.totalSeasons > 1 ? 's' : ''})`;
                            seriesSelect.appendChild(option);
                        });
                        
                        console.log('Series list loaded successfully:', result.data.length, 'series');
                    }
                }
            }
        } catch (error) {
            console.error('Error loading series list:', error);
            // Fallback data if API fails
            const seriesSelect = document.querySelector('select[name="seriesId"]');
            if (seriesSelect) {
                seriesSelect.innerHTML = `
                    <option value="">Choose a series</option>
                    <option value="1">Breaking Bad (5 seasons)</option>
                    <option value="2">Game of Thrones (8 seasons)</option>
                    <option value="3">The Office (9 seasons)</option>
                `;
            }
        }
    }

    async validateSlug(slug, type) {
        if (!slug) return;
        
        try {
            const response = await fetch('/Admin/ValidateSlug', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': this.getAntiForgeryToken()
                },
                body: JSON.stringify({ slug, type })
            });
            
            const result = await response.json();
            
            const slugInput = document.getElementById(`${type}-slug`);
            if (slugInput) {
                const parent = slugInput.parentElement;
                const existingMessage = parent.querySelector('.slug-validation');
                
                if (existingMessage) {
                    existingMessage.remove();
                }
                
                const message = document.createElement('p');
                message.className = 'slug-validation text-xs mt-1';
                message.textContent = result.message;
                
                if (result.isUnique) {
                    message.classList.add('text-green-600');
                    slugInput.classList.remove('border-red-500');
                    slugInput.classList.add('border-green-500');
                } else {
                    message.classList.add('text-red-600');
                    slugInput.classList.remove('border-green-500');
                    slugInput.classList.add('border-red-500');
                }
                
                parent.appendChild(message);
            }
        } catch (error) {
            console.error('Slug validation failed:', error);
        }
    }

    generateSlug(text) {
        return text
            .toLowerCase()
            .trim()
            .replace(/[^\w\s-]/g, '')
            .replace(/[\s_-]+/g, '-')
            .replace(/^-+|-+$/g, '');

    }

    async handleMovieUpload(formData) {
        try {
            this.showLoading('🎬 Preparing movie upload...');
            
            // 1. Reserve movie ID first
            const reservationResponse = await this.reserveMovieId(formData);
            if (!reservationResponse.success) {
                throw new Error(reservationResponse.error || 'Failed to reserve movie ID');
            }
            
            const { movieId, year } = reservationResponse;
            console.log(`🎬 Movie ID reserved: ${movieId}`);
            
            try {
                this.showLoading('📁 Uploading movie files to MinIO...');
                
                // 2. Upload files to MinIO with reserved movieId
                const fileUrls = await this.uploadFilesToMinIO(formData, 'movie', { movieId, year });
                
                // 3. Prepare movie data for backend
                const movieData = this.prepareMovieData(formData, fileUrls);
                movieData.reservedId = movieId; // Include reserved ID
                
                // 4. Send to backend with MinIO URLs
                const response = await this.sendToBackend('/Admin/UploadMovie', movieData);
                
                if (response.success) {
                    this.showSuccess('🎉 Movie uploaded successfully!');
                    this.resetForm('movieUploadForm');
                } else {
                    throw new Error(response.message || 'Upload failed');
                }
                
            } catch (uploadError) {
                // Cancel reserved ID if upload fails
                console.log('❌ Upload failed, cancelling reserved movie ID:', movieId);
                await this.cancelReservedId('movie', movieId);
                throw uploadError;
            }
            
        } catch (error) {
            this.showError(`❌ Upload failed: ${error.message}`);
        } finally {
            this.hideLoading();
        }
    }

    async handleSeriesUpload(formData) {
        try {
            this.showLoading('📺 Preparing series creation...');
            
            // 1. Reserve series ID first
            const reservationResponse = await this.reserveSeriesId(formData);
            if (!reservationResponse.success) {
                throw new Error(reservationResponse.error || 'Failed to reserve series ID');
            }
            
            const { seriesId, year } = reservationResponse;
            console.log(`📺 Series ID reserved: ${seriesId}`);
            
            try {
                this.showLoading('📁 Uploading series files to MinIO...');
                
                // 2. Upload images to MinIO with reserved seriesId
                const fileUrls = await this.uploadFilesToMinIO(formData, 'series', { seriesId, year });
                
                // 3. Prepare series data
                const seriesData = this.prepareSeriesData(formData, fileUrls);
                seriesData.reservedId = seriesId; // Include reserved ID
                
                // 4. Send to backend
                const response = await this.sendToBackend('/Admin/UploadSeries', seriesData);
                
                if (response.success) {
                    this.showSuccess('🎉 Series created successfully!');
                    this.resetForm('seriesUploadForm');
                    this.loadSeriesList(); // Refresh series list
                } else {
                    throw new Error(response.message || 'Upload failed');
                }
                
            } catch (uploadError) {
                // Cancel reserved ID if upload fails
                console.log('❌ Upload failed, cancelling reserved series ID:', seriesId);
                await this.cancelReservedId('series', seriesId);
                throw uploadError;
            }
            
        } catch (error) {
            this.showError(`❌ Upload failed: ${error.message}`);
        } finally {
            this.hideLoading();
        }
    }

    async handleEpisodeUpload(formData) {
        try {
            this.showLoading('📹 Uploading episode...');
            
            // For episodes, we use existing seriesId, seasonNumber, episodeNumber
            const seriesId = parseInt(formData.get('seriesId'));
            const seasonNumber = parseInt(formData.get('seasonNumber'));
            const episodeNumber = parseInt(formData.get('episodeNumber'));
            const year = new Date().getFullYear();
            
            if (!seriesId || !seasonNumber || !episodeNumber) {
                throw new Error('Series ID, season number, and episode number are required');
            }
            
            // 1. Upload files to MinIO with series info
            const fileUrls = await this.uploadFilesToMinIO(formData, 'episode', { seriesId, seasonNumber, episodeNumber, year });
            
            // 2. Prepare episode data
            const episodeData = this.prepareEpisodeData(formData, fileUrls);
            
            // 3. Send to backend
            const response = await this.sendToBackend('/Admin/UploadEpisode', episodeData);
            
            if (response.success) {
                this.showSuccess('🎉 Episode uploaded successfully!');
                this.resetForm('episodeUploadForm');
            } else {
                throw new Error(response.message || 'Upload failed');
            }
            
        } catch (error) {
            this.showError(`❌ Upload failed: ${error.message}`);
        } finally {
            this.hideLoading();
        }
    }

    async uploadFilesToMinIO(formData, contentType, metadata = {}) {
        const fileUrls = {};
        const fileTypes = this.getFileTypesForUpload(contentType);
        
        for (const fileType of fileTypes) {
            const file = formData.get(`${fileType}File`);
            if (file && file.size > 0) {
                try {
                    console.log(`🚀 Starting MinIO upload for ${fileType}: ${file.name}`);
                    
                    // Always use MinIO multipart upload for better reliability
                    const minioUrl = await this.uploadToMinIO(file, fileType, metadata);
                    fileUrls[`${fileType}Url`] = minioUrl;
                    
                    console.log(`✅ ${fileType} uploaded to MinIO: ${minioUrl}`);
                } catch (error) {
                    console.error(`❌ Failed to upload ${fileType}:`, error);
                    throw new Error(`Failed to upload ${fileType} file: ${error.message}`);
                }
            }
        }
        
        return fileUrls;
    }

    getFileTypesForUpload(type) {
        switch (type) {
            case 'movie':
                return ['video', 'poster', 'banner', 'subtitle'];
            case 'series':
                return ['poster', 'banner'];
            case 'episode':
                return ['video', 'subtitle'];
            default:
                return [];
        }
    }

    // MinIO Multipart Upload - Based on demo code with metadata support
    async uploadToMinIO(file, fileType, metadata = {}) {
        const progressElement = document.getElementById(`${this.currentTab}-${fileType}-progress`);
        if (progressElement) {
            progressElement.classList.remove('hidden');
        }

        console.log(`🚀 Starting MinIO multipart upload for ${file.name} (${this.formatFileSize(file.size)})`);
        
        try {
            // 1. Start multipart upload with metadata
            const { uploadId, parts, fileName: minioFileName } = await this.startMinIOUpload(file, fileType, metadata);
            
            if (!uploadId || !Array.isArray(parts) || parts.length === 0) {
                throw new Error("Invalid response from MinIO server.");
            }

            const normalizedParts = parts.map(p => ({
                PartNumber: p.partNumber || p.PartNumber,
                Url: p.url || p.Url
            }));
            
            let completed = 0;
            const results = [];
            const queue = [...normalizedParts];

            // 2. Upload parts with concurrency control
            const uploadPart = async (part) => {
                if (!part || !part.PartNumber || !part.Url) {
                    throw new Error("Invalid part data: " + JSON.stringify(part));
                }

                const start = (part.PartNumber - 1) * this.chunkSize;
                const end = Math.min(start + this.chunkSize, file.size);
                const blob = file.slice(start, end);

                console.log(`📤 Uploading part ${part.PartNumber} (${this.formatFileSize(blob.size)})`);
                console.log(`part.Url: ${part.Url}`)
                const res = await fetch(part.Url, { 
                    method: "PUT", 
                    body: blob,
                    headers: {
                        'Content-Type': file.type || 'application/octet-stream'
                    }
                });
                
                if (!res.ok) {
                    throw new Error(`Failed uploading part ${part.PartNumber}: ${res.status} ${res.statusText}`);
                }

                const eTag = res.headers.get("ETag")?.replace(/"/g, "");
                if (!eTag) {
                    throw new Error(`Missing ETag for part ${part.PartNumber}`);
                }

                results.push({ PartNumber: part.PartNumber, ETag: eTag });

                completed++;
                const progress = (completed / normalizedParts.length) * 100;
                this.updateProgress(fileType, progress);
                
                console.log(`✅ Part ${part.PartNumber} uploaded successfully (${completed}/${normalizedParts.length})`);
            };

            // 3. Run workers with concurrency limit
            const workers = Array(this.concurrency).fill(0).map(async () => {
                while (queue.length) {
                    const part = queue.shift();
                    try {
                        await uploadPart(part);
                    } catch (err) {
                        queue.length = 0; // Stop all workers
                        throw err;
                    }
                }
            });

            await Promise.all(workers);

            // 4. Complete multipart upload
            console.log(`🔗 Completing multipart upload for ${file.name}...`);
            const finalUrl = await this.completeMinIOUpload(minioFileName, uploadId, results, fileType, file.name);

            console.log(`🎉 Upload complete! URL: ${finalUrl}`);
            return finalUrl;

        } catch (error) {
            console.error(`❌ MinIO upload failed for ${file.name}:`, error);
            throw error;
        } finally {
            if (progressElement) {
                progressElement.classList.add('hidden');
            }
        }
    }

    // Start MinIO multipart upload with metadata
    async startMinIOUpload(file, fileType, metadata = {}) {
        const params = new URLSearchParams({
            fileName: file.name,
            fileSize: file.size.toString(),
            fileType: fileType,
            contentType: file.type || 'application/octet-stream'
        });

        // Add metadata for file path generation
        if (metadata.movieId) params.append('movieId', metadata.movieId.toString());
        if (metadata.seriesId) params.append('seriesId', metadata.seriesId.toString());
        if (metadata.seasonNumber) params.append('seasonNumber', metadata.seasonNumber.toString());
        if (metadata.episodeNumber) params.append('episodeNumber', metadata.episodeNumber.toString());
        if (metadata.year) params.append('year', metadata.year.toString());

        const response = await fetch(this.minioConfig.startEndpoint, {
            method: "POST",
            headers: { 
                "Content-Type": "application/x-www-form-urlencoded",
                'RequestVerificationToken': this.getAntiForgeryToken()
            },
            body: params.toString()
        });
        
        if (!response.ok) {
            throw new Error(`Failed to initiate MinIO upload: ${response.status} ${response.statusText}`);
        }
        
        const json = await response.json();
        console.log("🚀 MinIO start response:", json);
        return json;
    }

    // Complete MinIO multipart upload
    async completeMinIOUpload(minioFileName, uploadId, results, fileType, originalFileName) {
        const sortedParts = results.sort((a, b) => a.PartNumber - b.PartNumber);
        
        const response = await fetch(this.minioConfig.completeEndpoint, {
            method: "POST",
            headers: { 
                "Content-Type": "application/json",
                'RequestVerificationToken': this.getAntiForgeryToken()
            },
            body: JSON.stringify({
                fileName: minioFileName,
                originalFileName: originalFileName,
                uploadId: uploadId,
                parts: sortedParts,
                fileType: fileType
            })
        });
        
        if (!response.ok) {
            throw new Error(`Failed to complete MinIO upload: ${response.status} ${response.statusText}`);
        }
        
        const result = await response.json();
        console.log("✅ MinIO complete response:", result);
        
        return result.url || result.finalUrl || `http://localhost:9000/bucket/${minioFileName}`;
    }


    // Reserve movie ID before uploading
    async reserveMovieId(formData) {
        const title = formData.get('title');
        const slug = formData.get('slug');
        
        if (!title || !title.trim()) {
            throw new Error('Movie title is required');
        }
        
        const response = await fetch('/Admin/ReserveMovieId', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': this.getAntiForgeryToken()
            },
            body: JSON.stringify({
                title: title.trim(),
                slug: slug || null
            })
        });
        
        if (!response.ok) {
            throw new Error(`Failed to reserve movie ID: ${response.status} ${response.statusText}`);
        }
        
        const result = await response.json();
        console.log('🎬 Movie ID reservation response:', result);
        return result;
    }
    
    // Reserve series ID before uploading
    async reserveSeriesId(formData) {
        const title = formData.get('title');
        const slug = formData.get('slug');
        
        if (!title || !title.trim()) {
            throw new Error('Series title is required');
        }
        
        const response = await fetch('/Admin/ReserveSeriesId', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': this.getAntiForgeryToken()
            },
            body: JSON.stringify({
                title: title.trim(),
                slug: slug || null
            })
        });
        
        if (!response.ok) {
            throw new Error(`Failed to reserve series ID: ${response.status} ${response.statusText}`);
        }
        
        const result = await response.json();
        console.log('📺 Series ID reservation response:', result);
        return result;
    }
    
    // Cancel reserved ID if upload fails
    async cancelReservedId(type, id) {
        try {
            const response = await fetch('/Admin/CancelReservedId', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': this.getAntiForgeryToken()
                },
                body: JSON.stringify({
                    type: type,
                    id: id
                })
            });
            
            if (!response.ok) {
                console.error(`Failed to cancel reserved ${type} ID ${id}: ${response.status}`);
                return false;
            }
            
            const result = await response.json();
            console.log(`🗑️ Cancelled reserved ${type} ID ${id}:`, result);
            return result.success || false;
            
        } catch (error) {
            console.error(`Error cancelling reserved ${type} ID ${id}:`, error);
            return false;
        }
    }

    #endregion

    // Helper methods for progress updates, loading states, and messaging
    updateProgress(fileType, progress) {
        const progressElement = document.getElementById(`${this.currentTab}-${fileType}-progress`);
        if (progressElement) {
            const progressBar = progressElement.querySelector('.bg-blue-600');
            const progressText = progressElement.querySelector('p');
            
            if (progressBar) {
                progressBar.style.width = `${progress}%`;
            }
            
            if (progressText) {
                progressText.textContent = `Uploading... ${Math.round(progress)}%`;
            }
        }
    }

    showLoading(message) {
        // Find or create loading overlay
        let loadingOverlay = document.getElementById('loadingOverlay');
        if (!loadingOverlay) {
            loadingOverlay = document.createElement('div');
            loadingOverlay.id = 'loadingOverlay';
            loadingOverlay.className = 'fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50';
            loadingOverlay.innerHTML = `
                <div class="bg-white rounded-lg p-6 max-w-sm mx-4">
                    <div class="flex items-center space-x-4">
                        <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
                        <div>
                            <p class="text-lg font-medium text-gray-900" id="loadingMessage">${message}</p>
                            <p class="text-sm text-gray-500">Please wait...</p>
                        </div>
                    </div>
                </div>
            `;
            document.body.appendChild(loadingOverlay);
        } else {
            const messageElement = document.getElementById('loadingMessage');
            if (messageElement) {
                messageElement.textContent = message;
            }
            loadingOverlay.classList.remove('hidden');
        }
    }

    hideLoading() {
        const loadingOverlay = document.getElementById('loadingOverlay');
        if (loadingOverlay) {
            loadingOverlay.classList.add('hidden');
        }
    }

    showSuccess(message) {
        this.showNotification(message, 'success');
    }

    showError(message) {
        this.showNotification(message, 'error');
    }

    showNotification(message, type = 'info') {
        // Create notification
        const notification = document.createElement('div');
        notification.className = `fixed top-4 right-4 z-50 max-w-sm w-full bg-white border-l-4 rounded-lg shadow-lg p-4 ${
            type === 'success' ? 'border-green-500' : 
            type === 'error' ? 'border-red-500' : 
            'border-blue-500'
        }`;
        
        const iconClass = type === 'success' ? 'fas fa-check-circle text-green-500' : 
                         type === 'error' ? 'fas fa-exclamation-circle text-red-500' : 
                         'fas fa-info-circle text-blue-500';
        
        notification.innerHTML = `
            <div class="flex items-start">
                <div class="flex-shrink-0">
                    <i class="${iconClass}"></i>
                </div>
                <div class="ml-3 w-0 flex-1">
                    <p class="text-sm font-medium text-gray-900">${message}</p>
                </div>
                <div class="ml-4 flex-shrink-0 flex">
                    <button class="inline-flex text-gray-400 hover:text-gray-500" onclick="this.parentElement.parentElement.parentElement.remove()">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
        `;
        
        document.body.appendChild(notification);
        
        // Auto remove after 5 seconds
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    }

    formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    resetForm(formId) {
        const form = document.getElementById(formId);
        if (form) {
            form.reset();
            
            // Hide all previews
            const previews = form.querySelectorAll('[id$="-preview"]');
            previews.forEach(preview => {
                preview.classList.add('hidden');
            });
            
            // Clear video sources
            const videos = form.querySelectorAll('video');
            videos.forEach(video => {
                if (video.src) {
                    URL.revokeObjectURL(video.src);
                    video.src = '';
                }
            });
            
            // Clear image sources
            const images = form.querySelectorAll('img[id$="-image"]');
            images.forEach(img => {
                img.src = '';
            });
        }
    }

    getAntiForgeryToken() {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        return tokenInput ? tokenInput.value : '';
    }

    prepareMovieData(formData, fileUrls) {
        const movieData = {
            title: formData.get('title'),
            originalTitle: formData.get('originalTitle'),
            slug: formData.get('slug'),
            description: formData.get('description'),
            duration: parseInt(formData.get('duration')) || null,
            releaseDate: formData.get('releaseDate') || null,
            status: formData.get('status') || 'active',
            isPremium: formData.get('isPremium') === 'on',
            countryId: parseInt(formData.get('countryId')) || null,
            trailerUrl: formData.get('trailerUrl'),
            videoServerId: parseInt(formData.get('videoServerId')) || 1,
            videoQualityId: parseInt(formData.get('videoQualityId')) || 1,
            language: formData.get('language') || 'vi',
            categoryIds: this.getSelectedValues(formData.getAll('categoryIds')),
            directorIds: this.getSelectedValues(formData.getAll('directorIds')),
            actorIds: this.getSelectedValues(formData.getAll('actorIds')),
            // Add file URLs from MinIO
            ...fileUrls
        };
        
        return movieData;
    }

    prepareSeriesData(formData, fileUrls) {
        const seriesData = {
            title: formData.get('title'),
            originalTitle: formData.get('originalTitle'),
            slug: formData.get('slug'),
            description: formData.get('description'),
            totalSeasons: parseInt(formData.get('totalSeasons')) || 1,
            releaseDate: formData.get('releaseDate') || null,
            endDate: formData.get('endDate') || null,
            status: formData.get('status') || 'active',
            isPremium: formData.get('isPremium') === 'on',
            countryId: parseInt(formData.get('countryId')) || null,
            categoryIds: this.getSelectedValues(formData.getAll('categoryIds')),
            directorIds: this.getSelectedValues(formData.getAll('directorIds')),
            actorIds: this.getSelectedValues(formData.getAll('actorIds')),
            // Add file URLs from MinIO
            ...fileUrls
        };
        
        return seriesData;
    }

    prepareEpisodeData(formData, fileUrls) {
        const episodeData = {
            seriesId: parseInt(formData.get('seriesId')),
            seasonNumber: parseInt(formData.get('seasonNumber')),
            episodeNumber: parseInt(formData.get('episodeNumber')),
            title: formData.get('title'),
            description: formData.get('description'),
            duration: parseInt(formData.get('duration')) || null,
            airDate: formData.get('airDate') || null,
            isPremium: formData.get('isPremium') === 'on',
            videoServerId: parseInt(formData.get('videoServerId')) || 1,
            videoQualityId: parseInt(formData.get('videoQualityId')) || 1,
            language: formData.get('language') || 'vi',
            // Add file URLs from MinIO
            ...fileUrls
        };
        
        return episodeData;
    }

    getSelectedValues(values) {
        return values.filter(v => v && v.trim()).map(v => parseInt(v)).filter(v => !isNaN(v));
    }

    async sendToBackend(endpoint, data) {
        const response = await fetch(endpoint, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': this.getAntiForgeryToken()
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return await response.json();
    }

}

// Global functions for removing previews
window.removeImagePreview = function(formType, fileType) {
    const previewContainer = document.getElementById(`${formType}-${fileType}-preview`);
    const fileInput = document.getElementById(`${formType}-${fileType}`);
    
    if (previewContainer) {
        previewContainer.classList.add('hidden');
    }
    
    if (fileInput) {
        fileInput.value = '';
    }
};

window.removeVideoPreview = function(formType) {
    const previewContainer = document.getElementById(`${formType}-video-preview`);
    const videoElement = document.getElementById(`${formType}-video-player`);
    const fileInput = document.getElementById(`${formType}-video`);
    
    if (previewContainer) {
        previewContainer.classList.add('hidden');
    }
    
    if (videoElement && videoElement.src) {
        URL.revokeObjectURL(videoElement.src);
        videoElement.src = '';
    }
    
    if (fileInput) {
        fileInput.value = '';
    }
};

// Initialize when DOM is ready
let videoUploadManager;
document.addEventListener('DOMContentLoaded', () => {
    console.log('🚀 DOM loaded, initializing VideoUploadManager');
    videoUploadManager = new VideoUploadManager();
});

// Make it globally available
window.videoUploadManager = videoUploadManager;