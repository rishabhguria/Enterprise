<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Space">
		<xsl:param name="number"/>			
			<xsl:if test="$number &gt;0">
				<xsl:variable name="blank" select="''"/>
				<xsl:value-of select="concat($blank,' ')"/>				
				<xsl:call-template name="Space">
					<xsl:with-param name="number" select="$number - 1"/>
				</xsl:call-template>					
			</xsl:if>
	</xsl:template>

	<xsl:template name="FormatedDate">
		<xsl:param name="Date"/>
		
		<xsl:variable name="Year">
			<xsl:value-of select="substring-after(substring-after($Date,'/'),'/')"/>
		</xsl:variable>

		<xsl:variable name="Month">
			<xsl:choose>
				<xsl:when test="string-length(substring-before($Date,'/'))=1">
					<xsl:value-of select="concat('0',substring-before($Date,'/'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before($Date,'/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="Day">
			<xsl:choose>
				<xsl:when test="string-length(substring-before(substring-after($Date,'/'),'/'))=1">
					<xsl:value-of select="concat('0',substring-before(substring-after($Date,'/'),'/'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(substring-after($Date,'/'),'/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="concat($Year,$Month,$Day)"/>
		
	</xsl:template>
	
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select="'false'"/>
					</RowHeader>
					
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<ALLOCQTY>
						<xsl:value-of select="AllocatedQty"/>
					</ALLOCQTY>

					<xsl:variable name="RecordVersion" select="'103'"/>		

					<xsl:variable name="VendorID" select="'NIRV'"/>					

					<xsl:variable name="ProcessDate">
						<xsl:call-template name="FormatedDate">
							<xsl:with-param name="Date" select="ProcessDate"/>
						</xsl:call-template>						
					</xsl:variable>					

					<xsl:variable name="WireCode" select="'RT'"/>					

					<xsl:variable name="Transactioncounter" select="format-number(position(),'000000')"/>

					<xsl:variable name="Allocationcounter" select="format-number(position(),'0000')"/>					

					<xsl:variable name="Filler">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="20"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ClientID">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="RecordType" select="'AL  '"/>

					<xsl:variable name="ExecutionSource">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="LocationofExec1">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="LocationofExec2">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="LocationofExec3">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="LocationofExec4">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="LocationofExec5">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ClearanceType">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CorrespondentCapacity">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="3"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="UBSCapacity">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="3"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SettlementType" select="'RW'"/>

					<xsl:variable name="ProductType">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="PrimarySecondaryInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler2">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="26"/>
						</xsl:call-template>
					</xsl:variable>
					
					<!--next Column is Process date as we genrated in Third Column, so i again use same variable Process date-->

					<xsl:variable name="TradeDate">
						<xsl:call-template name="FormatedDate">
							<xsl:with-param name="Date" select="TradeDate"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SettlementDate">
						<xsl:call-template name="FormatedDate">
							<xsl:with-param name="Date" select="SettlementDate"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="TradeExecutionTime">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="6"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="JulianDate">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="5"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler3">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="25"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="AccountBLANK">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="13-string-length(AccountNo)"/>
						</xsl:call-template>
					</xsl:variable>

					<!--Need to change-->
					<xsl:variable name="AccountNumber">
						<xsl:value-of select="concat(' ',concat(AccountNo,$AccountBLANK))"/>
					</xsl:variable>

					<xsl:variable name="Side">
						<xsl:choose>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'T'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring(Side,1,1)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="ShortSaleStatus">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ShortSaleApproval">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ExecutedQuantity">
						<xsl:value-of select="translate(format-number(AllocatedQty,'0000000000.00000000'),'.','')"/>
					</xsl:variable>

					<xsl:variable name="PriceType" select="'Y'"/>

					<xsl:variable name="Price">
						<xsl:value-of select="translate(format-number(AveragePrice,'00000000.00000000'),'.','')"/>
					</xsl:variable>

					<xsl:variable name="Symbol">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="13"/>
						</xsl:call-template>
					</xsl:variable>

					<!--Find Blank Space when CUSIP length < 12-->
					<xsl:variable name="CUSIPBLANK">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="12-string-length(CUSIP)"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CUSIP">
						<xsl:value-of select="concat(CUSIP,$CUSIPBLANK)"/>
					</xsl:variable>

					<xsl:variable name="PrincipalAmount">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="17"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="UBSSymbol">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="9"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="UBSblotterCode">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="UBSProductCode">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="DiscretionInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="UBSSWPSleeveNo">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="3"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ManagerFOACode">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ValuationMethodforSell">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler4">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ExecBrokerNSCCSymbol">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ExecBrokerPnemonic">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ExecBrokerBadge">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OppExecBrkrNSCCSymbol">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler5">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="56"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OppExecBrokerPnemonic">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OppExecBrokerBadge">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OppClrngBrkr">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ExecClrngBrkr">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler6">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="34"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CommissionIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="PerShareCommission">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="12"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="TotalCommission">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="9"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="PercentageDiscAmount">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="5"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CommissionCalculationIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler7">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="29"/>
						</xsl:call-template>
					</xsl:variable>


					<xsl:variable name="OffsetAccount" select="' WU L9970  1LP'"/>




					<!--Need to change--><!--
					<xsl:variable name="OffsetAccountBlank">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="14-string-length('N/A')"/>
						</xsl:call-template>
					</xsl:variable>

					--><!--<xsl:variable name="OffsetAccount">
						<xsl:value-of select="concat('N/A',$OffsetAccountBlank)"/>
					</xsl:variable>-->

					<xsl:variable name="NetIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Markupdown">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="12"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SalesCreditType">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SalesCreditAmt">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="15"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="AddCreditType">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="AddCreditAmt">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="15"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler8">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="30"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="DOTIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="BBSSLocation">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SolicitedUnsol">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Remuneration">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SecFeeIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="WhenIssueInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ForeignSecInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="AckIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ActReportIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="NobookIndicator">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ListedIndicator" select="'L'"/>

					<xsl:variable name="CtrlNumber">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="10"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Receivetime">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="6"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OddLotInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="AsofACTInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SourceSystemInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SyndicateInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="StepoutInd" select="' '"/>

					<xsl:variable name="Filler9">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="3"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OriginalOrderID">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OriginalOrderNbr">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="DateOrderRcvd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="8"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OrderID">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="20"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler10">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="10"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="UserDefinedUniqueId">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="10"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="UBSTradeID">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="5"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OrigTradeDate">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="8"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="OrigSettleDate">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="8"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CxlCorrInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CxlNDInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler11">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="28"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ConfirmTrlr1">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="30"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ConfirmTrlr2">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="30"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ConfirmTrlr3">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="30"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="PSTrailer1">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="30"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="PSTrailer2">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="30"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="BranchID">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SequenceNumber">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="TraderID">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Terminal">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Desk">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler12">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="11"/>
						</xsl:call-template>
					</xsl:variable>
					
					<xsl:variable name="OpenCloseInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CustFirmInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CoverInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="StrikePrice">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="10"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SymbolBase">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="9"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="CallPutInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ExpirationMonth">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="2"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="DTCInd">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="DTC">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="4"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Optiontype">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="1"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="ErrorMessage">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="20"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="NSLISIN">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="12"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="Filler13">
						<xsl:call-template name="Space">
							<xsl:with-param name="number" select="50"/>
						</xsl:call-template>
					</xsl:variable>

					<COLUMN>
						<xsl:value-of select="concat($RecordVersion,$VendorID,$ProcessDate,$WireCode,$Transactioncounter,$Allocationcounter,$Filler,$ClientID
						,$RecordType,$ExecutionSource,$LocationofExec1,$LocationofExec2,$LocationofExec3,$LocationofExec4,$LocationofExec5,$ClearanceType
						,$CorrespondentCapacity,$UBSCapacity,$SettlementType,$ProductType,$PrimarySecondaryInd,$Filler2,$ProcessDate,$TradeDate,$SettlementDate
						,$TradeExecutionTime,$JulianDate,$Filler3,$AccountNumber,$Side,$ShortSaleStatus,$ShortSaleApproval,$ExecutedQuantity,$PriceType
						,$Price,$Symbol,$CUSIP,$PrincipalAmount,$UBSSymbol,$UBSblotterCode,$UBSProductCode,$DiscretionInd,$UBSSWPSleeveNo,$ManagerFOACode
						,$ValuationMethodforSell,$Filler4,$ExecBrokerNSCCSymbol,$ExecBrokerPnemonic,$ExecBrokerBadge,$OppExecBrkrNSCCSymbol,$Filler5
						,$OppExecBrokerPnemonic,$OppExecBrokerBadge,$OppClrngBrkr,$ExecClrngBrkr,$Filler6,$CommissionIndicator,$PerShareCommission
						,$TotalCommission,$PercentageDiscAmount,$CommissionCalculationIndicator,$Filler7,$OffsetAccount,$NetIndicator,$Markupdown
						,$SalesCreditType,$SalesCreditAmt,$AddCreditType,$AddCreditAmt,$Filler8,$DOTIndicator,$BBSSLocation,$SolicitedUnsol,$Remuneration
						,$SecFeeIndicator,$WhenIssueInd,$ForeignSecInd,$AckIndicator,$ActReportIndicator,$NobookIndicator,$ListedIndicator,$CtrlNumber
						,$Receivetime,$OddLotInd,$AsofACTInd,$SourceSystemInd,$SyndicateInd,$StepoutInd,$Filler9,$OriginalOrderID,$OriginalOrderNbr
						,$DateOrderRcvd,$OrderID,$Filler10,$UserDefinedUniqueId,$UBSTradeID,$OrigTradeDate,$OrigSettleDate,$CxlCorrInd,$CxlNDInd,$Filler11
						,$ConfirmTrlr1,$ConfirmTrlr2,$ConfirmTrlr3,$PSTrailer1,$PSTrailer2,$BranchID,$SequenceNumber,$TraderID,$Terminal,$Desk,$Filler12
						,$OpenCloseInd,$CustFirmInd,$CoverInd,$StrikePrice,$SymbolBase,$CallPutInd,$ExpirationMonth,$DTCInd,$DTC,$Optiontype,$ErrorMessage
						,$NSLISIN,$Filler13)"/>
					</COLUMN>
				
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

					<FileHeader>
						<xsl:value-of select="'TRUE'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'TRUE'"/>
					</FileFooter>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
