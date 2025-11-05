<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

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
    <xsl:param name="Month" />
    <xsl:param name="PutOrCall" />
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'X'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
 
  
  
  <xsl:template name="Option">
    <xsl:param name="varSymbol" />
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($varSymbol,' ')" />
    </xsl:variable>
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space($varSymbol),' ')" />
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),5,2)" />
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),3,2)" />
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),1,2)" />
    </xsl:variable>
    <xsl:variable name="PutORCall">
      
      <xsl:value-of select="translate($var, '0123456789.', '')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(translate($var,translate($var, '0123456789.', ''), ''),8,7) div 1000,'##.00')" />
    </xsl:variable>
    <xsl:variable name="MonthCodVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$ExpiryMonth" />
        <xsl:with-param name="PutOrCall" select="$PutORCall" />
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Day">
      <xsl:choose>
        <xsl:when test="substring($ExpiryDay,1,1)='0'">
          <xsl:value-of select="substring($ExpiryDay,2,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ExpiryDay" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
   <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
  </xsl:template>
	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL17"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition) and (contains(COL1,'D') or contains(COL9,'STK')or contains(COL9,'OPT'))">


					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'IB'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
							<xsl:variable name="varISIN">
							<xsl:value-of select="normalize-space(COL4)"/>
							</xsl:variable>
						
						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>
						
						<Symbol>
							<xsl:choose>
							    <xsl:when test="$PRANA_SYMBOL_NAME!=''">
							    	<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							    
				</xsl:when>	
                <xsl:when test="$varSymbol !='' or $varSymbol !='*'">
					<xsl:value-of select="$varSymbol"/>
				</xsl:when>		
								<xsl:when test="$varISIN !='' or $varISIN !='*'">
						   	        <xsl:value-of select="''"/>
						        </xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>					  

						<ISIN>
						<xsl:choose>
							<xsl:when test="$varISIN !='' or $varISIN !='*'">
								<xsl:value-of select="$varISIN"/>
							</xsl:when>
						   <xsl:otherwise>
						   	<xsl:value-of select="''"/>
						   </xsl:otherwise>
						</xsl:choose>
						</ISIN>	

						<xsl:variable name="PB_FUND_NAME" select="COL2"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<xsl:variable name="varAsset">
              <xsl:choose>
                <!-- <xsl:when test="contains($varAsset,':P:') or contains($varAsset,':C:')"> -->
				<xsl:when test="COL9 ='STK'">
                  <xsl:value-of select="'Equity'" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'EquityOption'" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL19"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="CostBasis1">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$CostBasis div $NetPosition"/>
							</xsl:call-template>
						</xsl:variable>

						<CostBasis>
						 <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                   <xsl:when test="$CostBasis1 &gt; 0">
									<xsl:value-of select="$CostBasis1 div 100"/>
								</xsl:when>
								<xsl:when test="$CostBasis1 &lt; 0">
									<xsl:value-of select="$CostBasis1 div (-100)"/>
								</xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$CostBasis1 &gt; 0">
									<xsl:value-of select="$CostBasis1 * 1"/>
								</xsl:when>
								<xsl:when test="$CostBasis1 &lt; 0">
									<xsl:value-of select="$CostBasis1 * (-1)"/>
								</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
							
						</CostBasis>
						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL21)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="SecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL20)"/>
							</xsl:call-template>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test="$SecFee &gt; 0">
									<xsl:value-of select="$SecFee"/>
								</xsl:when>
								<xsl:when test="$SecFee &lt; 0">
									<xsl:value-of select="$SecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
						
						  <xsl:variable name="varSide">
            <xsl:value-of select="normalize-space(COL16)"/>
						</xsl:variable>
            <SideTagValue>

              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when  test="$varSide='BUY'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='SHORT'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='SELL'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide='COVER'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when  test="$varSide ='BUY'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide ='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide ='SHORT'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide ='COVER'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
             
            </SideTagValue>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>
						<!--20241111-->
						
						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL12,7,2)"/>
						</xsl:variable>
						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL12,1,4)"/>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(COL12,5,2)"/>
						</xsl:variable>
						<xsl:variable name="varCurrentDate">
							<xsl:value-of select="concat($varMonth,'/',$varDay, '/', $varYear)"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select ="$varCurrentDate"/>
						</PositionStartDate>
						
						<xsl:variable name="varDay1">
							<xsl:value-of select="substring(COL14,7,2)"/>
						</xsl:variable>
						<xsl:variable name="varYear1">
							<xsl:value-of select="substring(COL14,1,4)"/>
						</xsl:variable>

						<xsl:variable name="varMonth1">
							<xsl:value-of select="substring(COL14,5,2)"/>
						</xsl:variable>
						<xsl:variable name="varCurrentDate1">
							<xsl:value-of select="concat($varMonth1,'/',$varDay1, '/', $varYear1)"/>
						</xsl:variable>
						<PositionSettlementDate>
							<xsl:value-of select ="$varCurrentDate1"/>
						</PositionSettlementDate>
						
						<xsl:variable name="FXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL24"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<xsl:variable name="varForexPrice">
							<xsl:choose>
								<xsl:when test ="$PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD' or $PB_CURRENCY_NAME='USD'">
									<xsl:value-of select="COL24 div COL23"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL23 div COL24"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="number($varForexPrice)">
									<xsl:value-of select="$varForexPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>
						<FXConversionMethodOperator>
							<xsl:choose>
								<xsl:when test="$PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD' or $PB_CURRENCY_NAME!='USD' ">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'M'"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXConversionMethodOperator>

						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL23)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:value-of select="5"/>
						</CounterPartyID>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>