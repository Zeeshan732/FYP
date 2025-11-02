import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Publication } from '../../models/api.models';

@Component({
  selector: 'app-publication-detail',
  templateUrl: './publication-detail.component.html',
  styleUrls: ['./publication-detail.component.scss']
})
export class PublicationDetailComponent implements OnInit {
  publication: Publication | null = null;
  loading = false;
  error = '';
  publicationId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.publicationId = +params['id'];
      if (this.publicationId) {
        this.loadPublication();
      }
    });
  }

  loadPublication() {
    if (!this.publicationId) return;

    this.loading = true;
    this.error = '';

    this.apiService.getPublication(this.publicationId).subscribe({
      next: (pub: Publication) => {
        this.publication = pub;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading publication:', error);
        this.error = 'Failed to load publication. Please try again.';
        this.loading = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/publications']);
  }

  openLink(url: string) {
    if (url) {
      window.open(url, '_blank');
    }
  }

  getDoiUrl(doi: string): string {
    if (!doi) return '';
    return `https://doi.org/${doi}`;
  }
}

