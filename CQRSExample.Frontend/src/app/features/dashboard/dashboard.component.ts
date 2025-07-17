import { Component, OnInit, ViewChild ,AfterViewInit} from '@angular/core';
import { DashboardService } from './services/dashboard.service';
import { DashboardStats } from './models/dashboard.model';
import { CommonModule } from '@angular/common';
import { NgApexchartsModule } from 'ng-apexcharts';
import { ChartComponent, ApexAxisChartSeries, ApexChart, ApexXAxis, ApexDataLabels, ApexTitleSubtitle, ApexPlotOptions, ApexNonAxisChartSeries, ApexResponsive, ApexLegend, ApexFill, ApexYAxis, ApexStroke } from 'ng-apexcharts';
import { GuestRegistrationService, MonthlyRegistrationDto, YearlyRegistrationFeeDto } from '../../core/api/guest-registration.service';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  title: ApexTitleSubtitle;
  plotOptions: ApexPlotOptions;
};

export type AreaChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  dataLabels: ApexDataLabels;
  title: ApexTitleSubtitle;
  stroke: ApexStroke;
  fill: ApexFill;
};

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  standalone: true,
  imports: [CommonModule, NgApexchartsModule]
})
export class DashboardComponent implements OnInit, AfterViewInit {
  @ViewChild("barChart") barChart!: ChartComponent;
  @ViewChild("areaChart") areaChart!: ChartComponent;

  public barChartOptions: Partial<ChartOptions> | any;
  public areaChartOptions: Partial<AreaChartOptions> | any;

  stats: DashboardStats | null = null;
   showChart :boolean = false;

  constructor(private dashboardService: DashboardService, private guestRegistrationService: GuestRegistrationService) {
    this.barChartOptions = {
      series: [{
        name: "Registrations",
        data: []
      }],
      chart: {
        height: 350,
        type: "bar",
        foreColor: '#ccc',
        colors: ['#ccc']
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '55%',
        }
      },
      dataLabels: {
        enabled: false
      },
      xaxis: {
        categories: [],
        labels: {
          style: {
            colors: '#ccc'
          }
        }
      },
      title: {
        text: "Monthly Guest Registrations",
        align: "left",
        style: {
          color: '#ccc'
        }
      }
    };

    this.areaChartOptions = {
      series: [{
        name: "Yearly Fee",
        data: []
      }],
      chart: {
        height: 350,
        type: "area",
        foreColor: '#ccc',
        toolbar: {
          show: false
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "smooth",
        colors: ['#ccc']
      },
      xaxis: {
        type: "category",
        categories: [],
        labels: {
          style: {
            colors: '#ccc'
          }
        }
      },
      yaxis: {
        labels: {
          formatter: function (val: any) {
            return val + " MMK";
          },
          style: {
            colors: '#ccc'
          }
        }
      },
      title: {
        text: "Yearly Registration Fees",
        align: "left",
        style: {
          color: '#ccc'
        }
      },
      fill: {
        type: "gradient",
        gradient: {
          shadeIntensity: 1,
          opacityFrom: 0.7,
          opacityTo: 0.9,
          stops: [0, 100],
          colors: ['#ccc'] // Green color
        }
      }
    };
  }

  ngOnInit(): void {
   this.getChartData();
  }

  ngAfterViewInit(): void {
    // This method can be used to perform actions after the view has been initialized
    this.getChartData();
  }



  getChartData(): void {
     this.dashboardService.getDashboardStats().subscribe(data => {
      this.stats = data;

      this.guestRegistrationService.getMonthlyRegistrations().subscribe((monthlyData: MonthlyRegistrationDto[]) => {
         this.showChart = true; // Show the chart after data is fetched
        this.barChartOptions = {
          ...this.barChartOptions,
          series: [{
            name: "Registrations",
            data: monthlyData.map(d => d.count)
          }],
          xaxis: {
            ...this.barChartOptions.xaxis,
            categories: monthlyData.map(d => d.month),
            labels: {
              style: {
                colors: '#ccc'
              }
            }
          },
          chart: {
            ...this.barChartOptions.chart,
            foreColor: '#ccc'
          },
          title: {
            ...this.barChartOptions.title,
            style: {
              color: '#ccc'
            }
          }
        };
        if (this.barChart) {

          console.log('Monthly data fetched:', monthlyData);
          setTimeout(() => {
            this.barChart.updateOptions(this.barChartOptions);
            console.log('Bar chart options updated:', this.barChartOptions);
          }, 0);
        }
      });

      this.guestRegistrationService.getYearlyRegistrationFees().subscribe((yearlyData: YearlyRegistrationFeeDto[]) => {
        this.areaChartOptions = {
          ...this.areaChartOptions,
          series: [{
            name: "Yearly Fee",
            data: yearlyData.map(d => d.totalFee)
          }],
          xaxis: {
            ...this.areaChartOptions.xaxis,
            categories: yearlyData.map(d => d.year),
            labels: {
              style: {
                colors: '#ccc'
              }
            }
          },
          yaxis: {
            ...this.areaChartOptions.yaxis,
            labels: {
              formatter: function (val: any) {
                return val + " MMK";
              },
              style: {
                colors: '#ccc'
              }
            }
          },
          chart: {
            ...this.areaChartOptions.chart,
            foreColor: '#ccc'
          },
          title: {
            ...this.areaChartOptions.title,
            style: {
              color: '#ccc'
            }
          },
          stroke: {
            ...this.areaChartOptions.stroke,
            colors: ['#ccc']
          }
        };
        if (this.areaChart) {
          console.log('Yearly data fetched:', yearlyData);
          setTimeout(() => {
            this.areaChart.updateOptions(this.areaChartOptions);
            console.log('Pie chart options updated:', this.areaChartOptions);
          }, 0);
        }
      });
    });
  }
}
