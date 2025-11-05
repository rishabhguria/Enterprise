<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''),'$',''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='02' ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='03' ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='04' ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='05' ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='06' ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07'  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='08'  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='09' ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='10' ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='11' ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='12' ">
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
        <xsl:when test="$Month='02' ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03' ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='04' ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='05' ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='06' ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='07'  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='08'  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='09' ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='10' ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='11' ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='12' ">
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
      <xsl:value-of select="substring(concat(COL1, COL2, COL3),1,3)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(concat(COL1, COL2, COL3),8,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(concat(COL1, COL2, COL3),6,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(concat(COL1, COL2, COL3),4,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(concat(COL1, COL2, COL3),10,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(concat(COL1, COL2, COL3),13) div 1000,'##.00')"/>
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
          <xsl:value-of select="substring($ExpiryDay,8,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ExpiryDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>

  </xsl:template>
	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				
				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:if test="number($varQuantity)">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varCUSIP" >
                            <xsl:value-of select="normalize-space(COL2)"/>
			            </xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>               
                                <xsl:when test="$varCUSIP !=''">
                                  <xsl:value-of select="''"/>
                                </xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$varCUSIP !=''">
									<xsl:value-of select="$varCUSIP"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<xsl:variable name="PB_FUND_NAME" select ="normalize-space(COL1)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>          

						 <Side>                      
                           <xsl:choose>
                             <xsl:when test="$varQuantity &gt; 0">
                               <xsl:value-of select ="'Buy'"/>
                             </xsl:when>
                             <xsl:when test="$varQuantity &lt; 0">
                               <xsl:value-of select ="'Sell short'"/>
                             </xsl:when>
                             <xsl:otherwise>
                               <xsl:value-of select="''"/>
                             </xsl:otherwise>
                           </xsl:choose>            
						 </Side>

						<xsl:variable name="varMarkPriceLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPriceLocal &gt; 0">
									<xsl:value-of select="$varMarkPriceLocal"/>
								</xsl:when>
								<xsl:when test="$varMarkPriceLocal &lt; 0">
									<xsl:value-of select="$varMarkPriceLocal * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="varNetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValue &gt; 0">
									<xsl:value-of select="$varNetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValue &lt; 0">
									<xsl:value-of select="$varNetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

                        <xsl:variable name="varMarketValueLocal">
                          <xsl:call-template name="Translate">
                            <xsl:with-param name="Number" select="COL14"/>
                          </xsl:call-template>
                        </xsl:variable>
                        
                        <MarketValue>
                          <xsl:choose>
                            <xsl:when test="number($varMarketValueLocal)">
                              <xsl:value-of select="$varMarketValueLocal"/>
                            </xsl:when>             
                            <xsl:otherwise>
                              <xsl:value-of select="0"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </MarketValue>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>


