<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

     <!-- <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='Equity' and IsSwapped='true'][CounterParty='NTRS']"> -->
      <!--<xsl:for-each select="ThirdPartyFlatFileDetail">-->
	  <xsl:for-each select="ThirdPartyFlatFileDetail[(contains(AccountName, 'Walleye')) or (AccountName= 'WMO Consumer TMT Systematic') or (AccountName= 'WOF Consumer TMT Systematic') or (AccountName= 'WOF Consumer TMT Discretionary') or (AccountName= 'WMO Consumer TMT Discretionary') or (AccountName= 'Swap - Walleye WOF TMT Discretionary') or (AccountName= 'Swap - Walleye WMO TMT Discretionary')][Asset='Equity' and IsSwapped='true']">

        <ThirdPartyFlatFileDetail>   

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

		  
		  <EXTERNALREFERENCEID>
            <xsl:choose>
              <xsl:when test ="TaxLotState = 'Allocated'">
                <xsl:value-of select="concat('Common','_',substring-before(substring-after(TradeDate,'/'),'/'),'_',EntityID)"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Amended'">
                <xsl:value-of select="concat('Common','_',substring-before(substring-after(TradeDate,'/'),'/'),'_',EntityID,'C')"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Deleted'">
                <xsl:value-of select="concat('Common','_',substring-before(substring-after(TradeDate,'/'),'/'),'_',EntityID,'X')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </EXTERNALREFERENCEID>
          
          <PREVEXTERNALREFERENCEID>
            <xsl:choose>
              <xsl:when test ="TaxLotState = 'Allocated'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Amended'">
                <xsl:value-of select="concat('Common','_',substring-before(substring-after(TradeDate,'/'),'/'),'_',EntityID)"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Deleted'">
                <xsl:value-of select="concat('Common','_',substring-before(substring-after(TradeDate,'/'),'/'),'_',EntityID)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PREVEXTERNALREFERENCEID>
          
          <MSGTYPE>
            <xsl:choose>
              <xsl:when test ="TaxLotState = 'Allocated'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Amended'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Deleted'">
                <xsl:value-of select="'X'"/>
              </xsl:when>
            </xsl:choose>
          </MSGTYPE>
          
          <DESK>
            <xsl:choose>
			  <xsl:when test="AccountName='Walleye WOF'">
                <xsl:value-of select="'WTAL'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WOF Systematic'">
                <xsl:value-of select="'WTAL'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye Concentrated'">
                <xsl:value-of select="'WTAL'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Discretionary'">
                <xsl:value-of select="'WTAL'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Systematic'">
                <xsl:value-of select="'WTAL'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WMO'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WMO Systematic'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WMO Consumer TMT Discretionary'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WMO Consumer TMT Systematic'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WOF TMT Discretionary'">
                <xsl:value-of select="'WTAL'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WMO TMT Discretionary'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			  
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </DESK>
          
          <SIDE>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SIDE>

          <QUANTITY>
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </QUANTITY>
          
          <PRICE>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PRICE>
          
          <EXECUTINGCOUNTERPARTY>
            <xsl:value-of select="CounterParty"/>
          </EXECUTINGCOUNTERPARTY>
          
          <TRADERID>
            <xsl:value-of select="'WTAL'"/>
          </TRADERID>

          
          
          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>


          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='BAML']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          <FUND>
            <xsl:choose>
			  <xsl:when test="AccountName='Walleye WOF'">
                <xsl:value-of select="'WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WOF Systematic'">
                <xsl:value-of select="'WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye Concentrated'">
                <xsl:value-of select="'WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Discretionary'">
                <xsl:value-of select="'WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Systematic'">
                <xsl:value-of select="'WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WMO'">
                <xsl:value-of select="'WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WMO Systematic'">
                <xsl:value-of select="'WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WMO Consumer TMT Discretionary'">
                <xsl:value-of select="'WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WMO Consumer TMT Systematic'">
                <xsl:value-of select="'WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WMO TMT Discretionary'">
                <xsl:value-of select="'WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WOF TMT Discretionary'">
                <xsl:value-of select="'WOMF'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FUND>

          <PBR>
		  <xsl:choose>
			  <xsl:when test="AccountName='Walleye WMO'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WMO Systematic'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WMO Consumer TMT Systematic'">
                <xsl:value-of select="'CHIM'"/>
              </xsl:when>
			   <xsl:when test="AccountName='WMO Consumer TMT Discretionary'">
                <xsl:value-of select="'CHIM2'"/>
              </xsl:when>
			 <xsl:when test="AccountName='Walleye WOF'">
                <xsl:value-of select="'MANAGEDACCOUNTS'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WOF Systematic'">
                <xsl:value-of select="'MANAGEDACCOUNTS'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye Concentrated'">
                <xsl:value-of select="'MANAGEDACCOUNTS'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Discretionary'">
                <xsl:value-of select="'MANAGEDACCOUNTS'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Systematic'">
                <xsl:value-of select="'MANAGEDACCOUNTS'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WMO TMT Discretionary'">
                <xsl:value-of select="'CHIM2'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WOF TMT Discretionary'">
                <xsl:value-of select="'MANAGEDACCOUNTS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'CHIM'"/>
              </xsl:otherwise>
            </xsl:choose>
          </PBR>
          
          <TRADEDATE>
            <xsl:value-of select="TradeDate"/>
          </TRADEDATE>
        
          <SETTLEDATE>
            <xsl:value-of select="SettlementDate"/>
          </SETTLEDATE>
          
          <INSTRUMENTIDENTIFIERTYPE>
		      <xsl:choose>
              <xsl:when test="Asset='Equity'">
            <xsl:value-of select="'ID_BB_GLOBAL'"/>
			    </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </INSTRUMENTIDENTIFIERTYPE>
          
		  
		  <INSTRUMENTIDENTIFIER>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                 <xsl:value-of select="SEDOL"/>
	        		</xsl:when>
			  
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </INSTRUMENTIDENTIFIER>
         
		 <CLEARINGACCOUNT>
            <xsl:choose>
			  <xsl:when test="AccountName='Walleye WOF'">
                <xsl:value-of select="'GSBCHI-DPB-WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WOF Systematic'">
                <xsl:value-of select="'GSBCHI-DPB-WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye Concentrated'">
                <xsl:value-of select="'GSBHI2-DPB-WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Discretionary'">
                <xsl:value-of select="'GSBHI3-DPB-WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WOF Consumer TMT Systematic'">
                <xsl:value-of select="'GSBHI3-DPB-WOMF'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WMO'">
                <xsl:value-of select="'GSCCHI-DPB-WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Walleye WMO Systematic'">
                <xsl:value-of select="'GSCCHI-DPB-WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WMO Consumer TMT Discretionary'">
                <xsl:value-of select="'GSCHI3-DPB-WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='WMO Consumer TMT Systematic'">
                <xsl:value-of select="'GSCHI3-DPB-WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WMO TMT Discretionary'">
                <xsl:value-of select="'GSCHI3-CFD-WMOP'"/>
              </xsl:when>
			  <xsl:when test="AccountName='Swap - Walleye WOF TMT Discretionary'">
                <xsl:value-of select="'GSBHI3-CFD-WOMF'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </CLEARINGACCOUNT>
          
          <CASHSUBACCOUNT>
            <xsl:value-of select="'FUTURE'"/>
          </CASHSUBACCOUNT>
          
          <ISTRS>
            <xsl:value-of select="'1'"/>
          </ISTRS>

          <STRATEGYID>
            <xsl:value-of select="'TRADING'"/>
          </STRATEGYID>
		  
          <FINANCIALTYPE>
		    	<xsl:choose>
              <xsl:when test="Asset='Equity'">
                  <xsl:value-of select="'COMMON'"/>
		       	</xsl:when>
			  
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
		  	</FINANCIALTYPE>

          <NOTES>
            <xsl:value-of select="concat('Common','_',substring-before(substring-after(TradeDate,'/'),'/'),'_',TradeRefID)"/>
          </NOTES>

          <xsl:variable name="varOtherFees">
            <xsl:value-of select="(StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
          </xsl:variable>
            
          <FEE_OTHER_FEE>
            <xsl:value-of select="$varOtherFees"/>
          </FEE_OTHER_FEE>


          <!--<xsl:variable name="varPershare">
            <xsl:value-of select="CommissionCharged div AllocatedQty"/>
          </xsl:variable>

          <xsl:variable name="varCommission" select="(CommissionCharged)+(SoftCommissionCharged)"/>
		  
          <FEE_EXEC_COMM>
            <xsl:value-of select="$varCommission"/>
          </FEE_EXEC_COMM>

          <FEE_SEC_FEE>
            <xsl:value-of select="SecFee"/>
          </FEE_SEC_FEE>

          <FEE_CLR_COMM>
            <xsl:value-of select="ClearingFee"/>
          </FEE_CLR_COMM>
			
			<FEE_ORF_FEE>
			<xsl:value-of select="OrfFee"/>
			</FEE_ORF_FEE>

          <FEE_OCC_FEE>
            <xsl:value-of select="OccFee"/>
          </FEE_OCC_FEE>-->

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>