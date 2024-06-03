namespace Orbita.Controles.VA
{
    partial class OGraficaVariableBool
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Fecha");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Valor");
            Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries numericTimeSeries1 = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect1 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.LineChartAppearance lineChartAppearance1 = new Infragistics.UltraChart.Resources.Appearance.LineChartAppearance();
            this.varDataSource = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ChartVariable = new Infragistics.Win.UltraWinChart.UltraChart();
            ((System.ComponentModel.ISupportInitialize)(this.varDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartVariable)).BeginInit();
            this.SuspendLayout();
            // 
            // varDataSource
            // 
            ultraDataColumn1.DataType = typeof(System.DateTime);
            ultraDataColumn2.DataType = typeof(int);
            this.varDataSource.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            this.varDataSource.Band.Key = "Valor";
            // 
            //'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
            //'ChartType' must be persisted ahead of any Axes change made in design time.
            //
            this.ChartVariable.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.StepLineChart;
            // 
            // ChartVariable
            // 
            this.ChartVariable.Axis.X.Extent = 59;
            this.ChartVariable.Axis.X.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.X.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ChartVariable.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL:MM-dd-yy>";
            this.ChartVariable.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ChartVariable.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ChartVariable.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.ChartVariable.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ChartVariable.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.X.LineThickness = 1;
            this.ChartVariable.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ChartVariable.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.X.MajorGridLines.Visible = true;
            this.ChartVariable.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ChartVariable.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.X.MinorGridLines.Visible = false;
            this.ChartVariable.Axis.X.TickmarkInterval = 50;
            this.ChartVariable.Axis.X.TickmarkIntervalType = Infragistics.UltraChart.Shared.Styles.AxisIntervalType.Hours;
            this.ChartVariable.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ChartVariable.Axis.X.Visible = false;
            this.ChartVariable.Axis.X2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.X2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ChartVariable.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.ChartVariable.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL:MM-dd-yy>";
            this.ChartVariable.Axis.X2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ChartVariable.Axis.X2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.X2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ChartVariable.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.ChartVariable.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.ChartVariable.Axis.X2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ChartVariable.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.X2.Labels.Visible = false;
            this.ChartVariable.Axis.X2.LineThickness = 1;
            this.ChartVariable.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ChartVariable.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.X2.MajorGridLines.Visible = true;
            this.ChartVariable.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ChartVariable.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.X2.MinorGridLines.Visible = false;
            this.ChartVariable.Axis.X2.TickmarkInterval = 50;
            this.ChartVariable.Axis.X2.TickmarkIntervalType = Infragistics.UltraChart.Shared.Styles.AxisIntervalType.Hours;
            this.ChartVariable.Axis.X2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ChartVariable.Axis.X2.Visible = false;
            this.ChartVariable.Axis.Y.Extent = 11;
            this.ChartVariable.Axis.Y.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Y.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ChartVariable.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.ChartVariable.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.ChartVariable.Axis.Y.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Y.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Y.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ChartVariable.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.ChartVariable.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.ChartVariable.Axis.Y.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Y.LineThickness = 1;
            this.ChartVariable.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ChartVariable.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Y.MajorGridLines.Visible = true;
            this.ChartVariable.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ChartVariable.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Y.MinorGridLines.Visible = false;
            this.ChartVariable.Axis.Y.TickmarkInterval = 1;
            this.ChartVariable.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
            this.ChartVariable.Axis.Y.Visible = true;
            this.ChartVariable.Axis.Y2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Y2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ChartVariable.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.ChartVariable.Axis.Y2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Y2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Y2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ChartVariable.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.ChartVariable.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Y2.Labels.Visible = false;
            this.ChartVariable.Axis.Y2.LineThickness = 1;
            this.ChartVariable.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ChartVariable.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Y2.MajorGridLines.Visible = true;
            this.ChartVariable.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ChartVariable.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Y2.MinorGridLines.Visible = false;
            this.ChartVariable.Axis.Y2.TickmarkInterval = 100000;
            this.ChartVariable.Axis.Y2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ChartVariable.Axis.Y2.Visible = false;
            this.ChartVariable.Axis.Z.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Z.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ChartVariable.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.Z.Labels.ItemFormatString = "";
            this.ChartVariable.Axis.Z.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Z.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Z.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ChartVariable.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.Z.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Z.Labels.Visible = false;
            this.ChartVariable.Axis.Z.LineThickness = 1;
            this.ChartVariable.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ChartVariable.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Z.MajorGridLines.Visible = true;
            this.ChartVariable.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ChartVariable.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Z.MinorGridLines.Visible = false;
            this.ChartVariable.Axis.Z.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ChartVariable.Axis.Z.Visible = false;
            this.ChartVariable.Axis.Z2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Z2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ChartVariable.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.Z2.Labels.ItemFormatString = "";
            this.ChartVariable.Axis.Z2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Z2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ChartVariable.Axis.Z2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ChartVariable.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ChartVariable.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ChartVariable.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ChartVariable.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ChartVariable.Axis.Z2.Labels.Visible = false;
            this.ChartVariable.Axis.Z2.LineThickness = 1;
            this.ChartVariable.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ChartVariable.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Z2.MajorGridLines.Visible = true;
            this.ChartVariable.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ChartVariable.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ChartVariable.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ChartVariable.Axis.Z2.MinorGridLines.Visible = false;
            this.ChartVariable.Axis.Z2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ChartVariable.Axis.Z2.Visible = false;
            this.ChartVariable.ColorModel.AlphaLevel = ((byte)(150));
            this.ChartVariable.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            numericTimeSeries1.Data.TimeValueColumn = "";
            numericTimeSeries1.Data.ValueColumn = "";
            numericTimeSeries1.Key = "Valor";
            this.ChartVariable.CompositeChart.Series.AddRange(new Infragistics.UltraChart.Data.Series.ISeries[] {
            numericTimeSeries1});
            this.ChartVariable.Data.DataMember = "Valor";
            this.ChartVariable.DataMember = "Valor";
            this.ChartVariable.DataSource = this.varDataSource;
            this.ChartVariable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartVariable.Effects.Effects.Add(gradientEffect1);
            lineChartAppearance1.EndStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.NoAnchor;
            lineChartAppearance1.MidPointAnchors = false;
            this.ChartVariable.LineChart = lineChartAppearance1;
            this.ChartVariable.Location = new System.Drawing.Point(0, 0);
            this.ChartVariable.Name = "ChartVariable";
            this.ChartVariable.Size = new System.Drawing.Size(777, 131);
            this.ChartVariable.TabIndex = 19;
            this.ChartVariable.TitleTop.Text = "dafasdf";
            this.ChartVariable.Tooltips.HighlightFillColor = System.Drawing.Color.DimGray;
            this.ChartVariable.Tooltips.HighlightOutlineColor = System.Drawing.Color.DarkGray;
            // 
            // CtrlVariableChartBool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChartVariable);
            this.Name = "CtrlVariableChartBool";
            ((System.ComponentModel.ISupportInitialize)(this.varDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartVariable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinDataSource.UltraDataSource varDataSource;
        private Infragistics.Win.UltraWinChart.UltraChart ChartVariable;
    }
}
