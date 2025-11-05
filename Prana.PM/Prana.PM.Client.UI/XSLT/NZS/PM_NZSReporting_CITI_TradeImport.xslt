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
	
		
  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test ="number($varQuantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'CITI'"/>
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
              <xsl:value-of select="normalize-space(COL22)"/>
            </xsl:variable>

        

			<Symbol>
				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
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
              <xsl:value-of select="normalize-space(COL1)"/>
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
		   		<xsl:value-of select="normalize-space(COL5)"/>
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
                <xsl:with-param name="Number" select="COL7"/>
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
              <xsl:value-of select="COL4"/>
            </xsl:variable>

		
			 <SideTagValue>
				<xsl:choose>
               <xsl:when test="$varSide='Buy'">
                 <xsl:value-of select="'1'"/>
               </xsl:when>
               <xsl:when test="$varSide='Sell'">
                 <xsl:value-of select="'2'"/>
               </xsl:when>
              <xsl:when test="$varSide='Sell short'">
                 <xsl:value-of select="'5'"/>
               </xsl:when>
			   
               <xsl:when test="$varSide='Buy to Close'">
                 <xsl:value-of select="'B'"/>
               </xsl:when>		   
             </xsl:choose>
            </SideTagValue>



            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select ="normalize-space(COL2)"/>
            </xsl:variable>
			  
            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

           <xsl:variable name="varSettlementDate">
              <xsl:value-of select ="''"/>
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
			
			  
			 <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
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
			
			 <xsl:variable name="varOrfFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
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
			
			  
			<xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
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
			  
		   <xsl:variable name="varTotalCommissionandFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
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
            </TotalCommissionandFees>
			  
			  
			<xsl:variable name="varStampDuty">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <StampDuty>
              <xsl:choose>
                <xsl:when test ="$varStampDuty &lt;0">
                  <xsl:value-of select ="$varStampDuty * -1"/>
                </xsl:when>
                <xsl:when test ="$varStampDuty &gt;0">
                  <xsl:value-of select ="$varStampDuty"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>