import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-technology-demo',
  templateUrl: './technology-demo.component.html',
  styleUrls: ['./technology-demo.component.scss']
})
export class TechnologyDemoComponent implements OnInit {
  selectedDemo: 'voice' | 'gait' | null = null;
  
  // Voice Analysis Properties
  audioFile: File | null = null;
  waveformBars: number[] = [];
  voiceAnalysisResults: any = null;
  
  // Gait Analysis Properties
  videoFile: File | null = null;
  gaitAnalysisResults: any = null;

  ngOnInit() {
    this.initializeWaveform();
  }

  @HostListener('window:scroll', ['$event'])
  onScroll() {
    this.observeElements();
  }

  selectDemo(demo: 'voice' | 'gait') {
    this.selectedDemo = demo;
    this.resetAnalysis();
  }

  // File Upload Methods
  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      if (this.selectedDemo === 'voice') {
        this.audioFile = files[0];
        this.generateWaveform();
      } else if (this.selectedDemo === 'gait') {
        this.videoFile = files[0];
      }
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.audioFile = file;
      this.generateWaveform();
    }
  }

  onVideoSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.videoFile = file;
    }
  }

  // Sample Data Methods
  loadSampleAudio(type: 'healthy' | 'parkinson') {
    this.audioFile = new File([''], `sample-${type}.wav`, { type: 'audio/wav' });
    this.generateWaveform();
    
    // Simulate analysis results
    setTimeout(() => {
      this.voiceAnalysisResults = this.getSampleVoiceResults(type);
    }, 2000);
  }

  loadSampleVideo(type: 'healthy' | 'parkinson') {
    this.videoFile = new File([''], `sample-${type}.mp4`, { type: 'video/mp4' });
    
    // Simulate analysis results
    setTimeout(() => {
      this.gaitAnalysisResults = this.getSampleGaitResults(type);
    }, 2000);
  }

  // Analysis Methods
  startVoiceAnalysis() {
    if (!this.audioFile) return;
    
    // Simulate processing
    this.voiceAnalysisResults = null;
    
    setTimeout(() => {
      this.voiceAnalysisResults = {
        confidence: Math.floor(Math.random() * 30) + 70, // 70-100%
        pitchStability: Math.floor(Math.random() * 40) + 60,
        rhythmConsistency: Math.floor(Math.random() * 35) + 65,
        volumeControl: Math.floor(Math.random() * 30) + 70,
        tremorDetection: Math.floor(Math.random() * 20) + 80,
        recommendations: [
          'Voice patterns show normal variation within healthy ranges',
          'Consider monitoring for subtle changes over time',
          'Regular assessment recommended for baseline establishment'
        ]
      };
    }, 3000);
  }

  startGaitAnalysis() {
    if (!this.videoFile) return;
    
    // Simulate processing
    this.gaitAnalysisResults = null;
    
    setTimeout(() => {
      this.gaitAnalysisResults = {
        overallScore: Math.floor(Math.random() * 25) + 75, // 75-100%
        stepLength: Math.floor(Math.random() * 30) + 70,
        cadence: Math.floor(Math.random() * 25) + 75,
        balance: Math.floor(Math.random() * 35) + 65,
        swingPhase: Math.floor(Math.random() * 20) + 80,
        recommendations: [
          'Gait patterns indicate normal motor function',
          'Step length and cadence within expected ranges',
          'Consider regular monitoring for early detection'
        ]
      };
    }, 3000);
  }

  // Utility Methods
  private initializeWaveform() {
    this.waveformBars = Array.from({ length: 50 }, () => Math.random() * 100);
  }

  private generateWaveform() {
    this.waveformBars = Array.from({ length: 100 }, () => Math.random() * 120 + 20);
  }

  private resetAnalysis() {
    this.audioFile = null;
    this.videoFile = null;
    this.voiceAnalysisResults = null;
    this.gaitAnalysisResults = null;
    this.waveformBars = [];
  }

  private getSampleVoiceResults(type: 'healthy' | 'parkinson') {
    if (type === 'healthy') {
      return {
        confidence: 92,
        pitchStability: 88,
        rhythmConsistency: 91,
        volumeControl: 85,
        tremorDetection: 15,
        recommendations: [
          'Voice patterns indicate healthy neurological function',
          'Pitch stability and rhythm are within normal ranges',
          'Minimal tremor detected - within expected variation'
        ]
      };
    } else {
      return {
        confidence: 78,
        pitchStability: 65,
        rhythmConsistency: 72,
        volumeControl: 68,
        tremorDetection: 45,
        recommendations: [
          'Voice patterns suggest potential neurological considerations',
          'Pitch instability and rhythm variations detected',
          'Moderate tremor presence - recommend clinical evaluation'
        ]
      };
    }
  }

  private getSampleGaitResults(type: 'healthy' | 'parkinson') {
    if (type === 'healthy') {
      return {
        overallScore: 89,
        stepLength: 85,
        cadence: 92,
        balance: 88,
        swingPhase: 91,
        recommendations: [
          'Gait patterns indicate normal motor function',
          'Step length and cadence within healthy ranges',
          'Excellent balance and swing phase coordination'
        ]
      };
    } else {
      return {
        overallScore: 67,
        stepLength: 58,
        cadence: 72,
        balance: 61,
        swingPhase: 68,
        recommendations: [
          'Gait patterns show signs of motor dysfunction',
          'Reduced step length and cadence variations',
          'Balance and coordination concerns - recommend assessment'
        ]
      };
    }
  }

  private observeElements() {
    const elements = document.querySelectorAll('.demo-card');
    
    elements.forEach(element => {
      const elementTop = element.getBoundingClientRect().top;
      const elementVisible = 150;

      if (elementTop < window.innerHeight - elementVisible) {
        element.classList.add('animate');
      }
    });
  }
}