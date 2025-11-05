<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  
  <xsl:template name="Count">
    <xsl:param name="Symbol"/>
    <xsl:value-of select="count(//ThirdPartyFlatFileDetail[Symbol=$Symbol])"/>
  </xsl:template>

  <xsl:template name="SumPrice">
    <xsl:param name="Symbol"/>
    <xsl:value-of select="sum(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/AveragePrice)"/>
  </xsl:template>

  <xsl:template name="BLK">
    <xsl:param name="ID"/>
    <xsl:param name="Symbol"/>
    <xsl:choose>
      <xsl:when test="$ID = (//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)">
        <xsl:value-of select="(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
	  
	        <xsl:for-each select="ThirdPartyFlatFileDetail[Asset !='FX' and Asset !='FXForward' and  contains(AccountName,'Karst') 
			and (UnderLyingName = 'EU')]">

        <ThirdPartyFlatFileDetail>

          <xsl:variable name="Count">
            <xsl:call-template name="Count">
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="SumPrice">
            <xsl:call-template name ="SumPrice">
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="AvgofPrice">
            <xsl:value-of select="AveragePrice"/>
          </xsl:variable>

          <xsl:variable name="BLK">
            <xsl:call-template name="BLK">
              <xsl:with-param name="ID" select="PBUniqueID"/>
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>


         
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
		  
           <Acct>
		   <xsl:choose>
		     <xsl:when test="contains(AccountName,'Mult')">
                <xsl:value-of select="'Mult'"/>
              </xsl:when>		
              <xsl:otherwise>
                <xsl:value-of select="AccountName"/>
              </xsl:otherwise>
            </xsl:choose> 
          </Acct>

          <BuySell>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'BO'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'SO'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'SC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </BuySell>

          <Completed>
			 <xsl:value-of select="format-number(AllocatedQty,'###,###.####')"/>
          </Completed>

          <Description>
            <xsl:value-of select="FullSecurityName"/>
          </Description>

          <Symbol>
		   <xsl:choose>
		    <xsl:when test="contains(substring-after(substring-after(BBCode,' '),' '),'Equity') ">
                <xsl:value-of select ="substring-before(BBCode,' Equity')"/>
              </xsl:when>
              <xsl:when test="contains(substring-after(substring-after(BBCode,' '),' '),'EQUITY') ">
                <xsl:value-of select ="substring-before(BBCode,' EQUITY')"/>
              </xsl:when>
			  <xsl:when test="contains(substring-after(substring-after(BBCode,' '),' '),'equity') ">
                <xsl:value-of select ="substring-before(BBCode,' equity')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>              
          </Symbol>

          <xsl:variable name="varSettFxAmt">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Trade ='M'">
                    <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AveragePrice"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varAvgPrice">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <ExPrice>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="format-number($varAvgPrice,'###,##0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>           
          </ExPrice>


          <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varGrossAmount">
            <xsl:value-of select="GrossAmount"/>
          </xsl:variable>

          <xsl:variable name="Grossamount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$varGrossAmount"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$varGrossAmount * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$varGrossAmount div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <GrossVal>
            <xsl:choose>
              <xsl:when test="number($Grossamount)">			 
                <xsl:value-of select="format-number($Grossamount,'###,###.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>            
          </GrossVal>


          <xsl:variable name="varNetAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="NetAmount * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="NetAmount div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <NetVal>
            <xsl:choose>
              <xsl:when test="number($varNetAmount)">
                <xsl:value-of select="format-number($varNetAmount,'###,###.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>            
          </NetVal>


          <SettleCcy>
            <xsl:value-of select="SettlCurrency"/>
          </SettleCcy>

          <xsl:variable name="VarFxrate">
            <xsl:choose>
              <xsl:when test="FXRate_Taxlot='0'">
                <xsl:value-of select ="1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <FXRate>
            <xsl:value-of select="format-number($VarFxrate,'0.####')"/>
          </FXRate>

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>

          <TradeDate>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </SettleDate>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
