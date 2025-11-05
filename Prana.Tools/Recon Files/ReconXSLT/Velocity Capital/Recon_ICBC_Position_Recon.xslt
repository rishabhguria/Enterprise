<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
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
    <xsl:param name="Suffix"/>
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space($Symbol),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after(normalize-space($Symbol),' '),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after(normalize-space($Symbol),' '),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after(normalize-space($Symbol),' '),1,2)"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(normalize-space($Symbol),' '),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        
		 <xsl:choose>
                <xsl:when test ="format-number(substring(substring-after(normalize-space($Symbol),' '),8) div 1000,'##.00') &gt; 1">
                  <xsl:value-of select ="format-number(substring(substring-after(normalize-space($Symbol),' '),8) div 1000,'##.00')"/>
                </xsl:when>
				  <xsl:when test ="format-number(substring(substring-after(normalize-space($Symbol),' '),8) div 1000,'##.00') = 1">
          <xsl:value-of select ="format-number(substring(substring-after(normalize-space($Symbol),' '),8) div 1000,'##.00')"/>
        </xsl:when>
                <xsl:otherwise>
                 <xsl:value-of select="concat('0',format-number(substring(substring-after(normalize-space($Symbol),' '),8) div 1000,'##.00'))"/>
                </xsl:otherwise>
              </xsl:choose>
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

      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>

  </xsl:template>
  
  <xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'Velocity'"/>
				</xsl:variable>

				<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="concat(COL2,COL3,COL4,COL5,COL6)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						
				<xsl:if test ="number(COL12)">

					<PositionMaster>



						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL21"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								 <xsl:when test="COL11='OS'">
                      <xsl:call-template name="Option">
                        <xsl:with-param name="Symbol" select="COL9"/>
                      </xsl:call-template>
                    </xsl:when>
					 <xsl:when test="COL11='OI'">
                      <xsl:call-template name="Option">
                        <xsl:with-param name="Symbol" select="COL9"/>
                      </xsl:call-template>
                    </xsl:when>
								
								<xsl:otherwise>
									 <xsl:value-of select ="COL9"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						

						 <PBSymbol>
              <xsl:choose>
                <xsl:when test ="COL21 !='*'">
                  <xsl:value-of select ="concat('Description: ',COL21, ', Symbol: ', COL9)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL9"/>
                </xsl:otherwise>
              </xsl:choose>
            </PBSymbol>



						
						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								
								<xsl:when test ="COL6='*'">
							<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
								</xsl:when>
								<xsl:when test ="COL6=' '">
							<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
								</xsl:when>
								
								<xsl:when test ="COL6=''">
							<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5),'-',normalize-space(COL6))"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<CompanyName>
							<xsl:choose>
								<xsl:when test ="COL21 !=''">
									<xsl:value-of select ="COL21"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL9"/>
								</xsl:otherwise>
							</xsl:choose>
						</CompanyName>

						<CurrencySymbol>
							<xsl:value-of select ="COL8"/>
						</CurrencySymbol>

						<xsl:variable name ="varQuantity">
							<xsl:value-of select ="number(COL12)"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>

								<xsl:when test ="number($varQuantity)">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>


						<Side>
							<xsl:choose>
								<xsl:when test="COL11='OS' and $varQuantity &gt; 0">
									<xsl:value-of select ="'Buy to open'"/>
								</xsl:when>
								<xsl:when test="COL11='OS' and $varQuantity &lt; 0">
									<xsl:value-of select ="'Sell to close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$varQuantity &gt; 0">
											<xsl:value-of select ="'Buy'"/>
										</xsl:when>
										<xsl:when test ="$varQuantity &lt; 0">
											<xsl:value-of select ="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="varAvgPX">
							<xsl:value-of select="number(COL16)"/>
						</xsl:variable>

				

						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(COL14)"/>
						</xsl:variable>
						
						<AvgPX>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>
								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>
								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<MarketValueBase>

							<xsl:value-of select ="COL15"/>
							<!--<xsl:choose>
								<xsl:when test ="COL15 &lt;0">
									<xsl:value-of select ="COL15*-1"/>
								</xsl:when>
								<xsl:when test ="COL15 &gt;0">
									<xsl:value-of select ="COL15"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</MarketValueBase>

						<NetNotionalValue>

							<xsl:choose>
								<xsl:when test ="COL12 &lt;0">
									<xsl:value-of select ="COL17*-1"/>
								</xsl:when>
								<xsl:when test ="COL12 &gt;0">
									<xsl:value-of select ="COL17"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</NetNotionalValue>

						<TradeDate>
							<xsl:value-of select="COL1"/>
						</TradeDate>

						<SMRequest>
							<xsl:value-of select ="'true'"/>
						</SMRequest>

						</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>


	</xsl:template>

</xsl:stylesheet>
