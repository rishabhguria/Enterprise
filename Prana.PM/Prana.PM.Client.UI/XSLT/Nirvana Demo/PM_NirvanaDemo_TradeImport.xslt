<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>
	
		<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(COL5),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),5,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),3,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),1,2)"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),7,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL5),' '),8,8) div 1000,'#.00')"/>

		</xsl:variable>
		<xsl:variable name="MonthCodVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$ExpiryMonth"/>
				<xsl:with-param name="PutOrCall" select="$PutORCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="substring($ExpiryDay,1,1)='0'">
					<xsl:value-of select="substring($ExpiryDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ExpiryDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>

	</xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test ="number($varQuantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
			  
			<xsl:variable name="varAsset">
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
				<xsl:choose>
					<xsl:when test="varAsset='EquityOption'">
						<xsl:value-of select="'EquityOption'"/>
					</xsl:when>								
					<xsl:otherwise>
						<xsl:value-of select="'Equity'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<Symbol>
				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					</xsl:when>
					<xsl:when test="$varAssetType='EquityOption'">
						<xsl:call-template name="Option">
							<xsl:with-param name="Symbol" select="normalize-space(COL5)"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:when test="$varSymbol!=''">
						<xsl:value-of select="$varSymbol"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</Symbol>


            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>
			  
			  
			<xsl:variable name="PB_BROKER_NAME">
		   		<xsl:value-of select="normalize-space(COL6)"/>
		   	</xsl:variable>
		   	
		   	<xsl:variable name="PRANA_BROKER_ID">
		   		<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
		   	</xsl:variable>
		   	
		   	<CounterPartyID>
		   		<xsl:choose>
		   			<xsl:when test="number($PRANA_BROKER_ID!='')">
		   				<xsl:value-of select="$PRANA_BROKER_ID"/>
		   			</xsl:when>
		   			<xsl:otherwise>
		   				<xsl:value-of select="0"/>
		   			</xsl:otherwise>
		   		</xsl:choose>
		   	</CounterPartyID>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
           
            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test ="$varCostBasis &lt;0">
                  <xsl:value-of select ="$varCostBasis* -1"/>
                </xsl:when>
                <xsl:when test ="$varCostBasis &gt;0">
                  <xsl:value-of select ="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
				  <xsl:when test="$varSide='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select ="normalize-space(COL1)"/>
            </xsl:variable>
			  
            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

           <xsl:variable name="varSettlementDate">
              <xsl:value-of select ="normalize-space(COL2)"/>
            </xsl:variable>

            <PositionSettlementDate>
              <xsl:value-of select ="$varSettlementDate"/>
            </PositionSettlementDate>
			  
			<xsl:variable name="varSettlCurrencyName">
               <xsl:value-of select ="normalize-space(COL10)"/>
            </xsl:variable>

            <SettlCurrencyName>
              <xsl:value-of select ="$varSettlCurrencyName"/>
            </SettlCurrencyName>
			  
		    <xsl:variable name="varSoftCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>

            <SoftCommission>
              <xsl:choose>
                <xsl:when test ="$varSoftCommission &lt;0">
                  <xsl:value-of select ="$varSoftCommission* -1"/>
                </xsl:when>
                <xsl:when test ="$varSoftCommission &gt;0">
                  <xsl:value-of select ="$varSoftCommission"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SoftCommission>
			  
			 <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>
              <xsl:choose>
                <xsl:when test ="$varCommission &lt;0">
                  <xsl:value-of select ="$varCommission* (-1)"/>
                </xsl:when>
                <xsl:when test ="$varCommission &gt;0">
                  <xsl:value-of select ="$varCommission"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>		
			  
			<xsl:variable name="varClearingBrokerFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>

            <ClearingBrokerFees>
              <xsl:choose>
                <xsl:when test ="$varClearingBrokerFee &lt;0">
                  <xsl:value-of select ="$varClearingBrokerFee* -1"/>
                </xsl:when>
                <xsl:when test ="$varClearingBrokerFee &gt;0">
                  <xsl:value-of select ="$varClearingBrokerFee"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </ClearingBrokerFees>
			  
			  
			 <xsl:variable name="varOrfFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <OrfFee>
              <xsl:choose>
                <xsl:when test ="$varOrfFee &lt;0">
                  <xsl:value-of select ="$varOrfFee* -1"/>
                </xsl:when>
                <xsl:when test ="$varOrfFee &gt;0">
                  <xsl:value-of select ="$varOrfFee"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrfFee>
			  
		    <xsl:variable name="varOccFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <OccFee>
              <xsl:choose>
                <xsl:when test ="$varOccFee &lt;0">
                  <xsl:value-of select ="$varOccFee* -1"/>
                </xsl:when>
                <xsl:when test ="$varOccFee &gt;0">
                  <xsl:value-of select ="$varOccFee"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OccFee>
			  
			<xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL16"/>
              </xsl:call-template>
            </xsl:variable>

            <SecFee>
              <xsl:choose>
                <xsl:when test ="$varSecFee &lt;0">
                  <xsl:value-of select ="$varSecFee* -1"/>
                </xsl:when>
                <xsl:when test ="$varSecFee &gt;0">
                  <xsl:value-of select ="$varSecFee"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>
			  
		    <!--<xsl:variable name="varTotalCommissionandFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>

            <TotalCommissionandFees>
              <xsl:choose>
                <xsl:when test ="$varTotalCommissionandFee &lt;0">
                  <xsl:value-of select ="$varTotalCommissionandFee* -1"/>
                </xsl:when>
                <xsl:when test ="$varTotalCommissionandFee &gt;0">
                  <xsl:value-of select ="$varTotalCommissionandFee"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TotalCommissionandFees>-->
			  
			  
			<xsl:variable name="varOtherBrokerFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
              </xsl:call-template>
            </xsl:variable>

            <OtherBrokerFees>
              <xsl:choose>
                <xsl:when test ="$varOtherBrokerFees &lt;0">
                  <xsl:value-of select ="$varOtherBrokerFees* -1"/>
                </xsl:when>
                <xsl:when test ="$varOtherBrokerFees &gt;0">
                  <xsl:value-of select ="$varOtherBrokerFees"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OtherBrokerFees>


            <!--<xsl:variable name="varNetAmountBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>

            <NetAmountBase>
              <xsl:choose>
                <xsl:when test ="$varNetAmountBase &lt;0">
                  <xsl:value-of select ="$varNetAmountBase* -1"/>
                </xsl:when>
                <xsl:when test ="$varNetAmountBase &gt;0">
                  <xsl:value-of select ="$varNetAmountBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetAmountBase>-->
			  
			  
		  
			  			  	

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>