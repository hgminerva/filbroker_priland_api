using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnSoldUnit
    {
        public Int32 Id { get; set; }
        public String SoldUnitNumber { get; set; }
        public String SoldUnitDate { get; set; }
        public Int32 ProjectId { get; set; }
        public String Project { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public Int32 BrokerId { get; set; }
        public String Broker { get; set; }
        public String Agent { get; set; }
        public String BrokerCoordinator { get; set; }
        public Int32 ChecklistId { get; set; }
        public String Checklist { get; set; }
        public Decimal MiscellaneousFeeAmount { get; set; }
        public Decimal VATAmount { get; set; }
        public Decimal Price { get; set; }
        public Decimal PriceDiscount { get; set; }
        public Decimal DownpaymentValue { get; set; }
        public Decimal DownpaymentPercent { get; set; }
        public Decimal TCP { get; set; }
        public Decimal TSP { get; set; }
        public Decimal EquityValue { get; set; }
        public Decimal EquityPercent { get; set; }
        public Decimal EquitySpotPayment1 { get; set; }
        public Decimal EquitySpotPayment2 { get; set; }
        public Decimal EquitySpotPayment3 { get; set; }
        public Int32 EquitySpotPayment1Pos { get; set; }
        public Int32 EquitySpotPayment2Pos { get; set; }
        public Int32 EquitySpotPayment3Pos { get; set; }
        public Decimal Discount { get; set; }
        public Decimal DiscountedEquity { get; set; }
        public Decimal Reservation { get; set; }
        public Decimal NetEquity { get; set; }
        public Decimal NetEquityBalance { get; set; }
        public Decimal NetEquityInterest { get; set; }
        public Decimal NetEquityNoOfPayments { get; set; }
        public Decimal NetEquityAmortization { get; set; }
        public Decimal Balance { get; set; }
        public Decimal BalanceInterest { get; set; }
        public Decimal BalanceNoOfPayments { get; set; }
        public Decimal BalanceAmortization { get; set; }
        public String TotalInvestment { get; set; }
        public String PaymentOptions { get; set; }
        public String Financing { get; set; }
        public String Remarks { get; set; }
        public String FinancingType { get; set; }
        public Int32 PreparedBy { get; set; }
        public String PreparedByUser { get; set; }
        public Int32 CheckedBy { get; set; }
        public String CheckedByUser { get; set; }
        public Int32 ApprovedBy { get; set; }
        public String ApprovedByUser { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
        public Decimal? PriceBalance { get; set; }
        public Decimal? PricePayment { get; set; }

        public String LastPaymentDate { get; set; }
        public Decimal Ratio { get; set; }
    }
}