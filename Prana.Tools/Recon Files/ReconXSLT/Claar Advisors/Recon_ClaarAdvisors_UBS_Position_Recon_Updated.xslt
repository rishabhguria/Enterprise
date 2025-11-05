<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
	
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
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

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL10"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="normalize-space(COL9)='Equity' or normalize-space(COL9)='Bond' or normalize-space(COL9)='Option'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>
									  
							<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test="normalize-space(COL9)='Option'">
								<xsl:value-of select="'EquityOption'" />
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='Bond'">
								<xsl:value-of select="'FixedIncome'" />
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='Equity'">
									<xsl:value-of select="'Equity'" />
								</xsl:when>
								<xsl:otherwise>
								<xsl:value-of select="''" />
								</xsl:otherwise>
							</xsl:choose>
							</xsl:variable>
							
							<Asset>
								<xsl:value-of select ="$varAssetType"/>
							</Asset>
						
						    <xsl:variable name="varCUSIP">
						    	<xsl:choose>
						    		<xsl:when test="$varAssetType = 'FixedIncome'">
						    			<xsl:value-of select="normalize-space(COL8)"/>
						    		</xsl:when>
						    		<xsl:otherwise>
						    			<xsl:value-of select="''"/>
						    		</xsl:otherwise>
						    	</xsl:choose>
						    </xsl:variable>
						
						<xsl:variable name="varSedol">
							<xsl:choose>
								<xsl:when test="$varAssetType = 'Equity'">
									<xsl:value-of select="normalize-space(COL7)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varISIN">
						   <xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>
						
						<xsl:variable name="varSymbol">
						   <xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>
							
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
			

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSedol !='' ">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varCUSIP !=''">
									<xsl:value-of select="''"/>
								</xsl:when>
							    <xsl:when test="$varAssetType='EquityOption'">
							    	<xsl:call-template name="Option">
							    		<xsl:with-param name="varSymbol" select="normalize-space(COL4)" />
							    	</xsl:call-template>
							    </xsl:when>								
								<xsl:when test="$varSymbol !='*' or $varSymbol !=''">
										<xsl:value-of select="$varSymbol"/>
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
            
			            <SEDOL> 
                          <xsl:choose> 
                           <xsl:when test="$varSedol!=''"> 
                              <xsl:value-of select="$varSedol"/> 
                            </xsl:when> 
                            <xsl:otherwise> 
                              <xsl:value-of select="''"/> 
                            </xsl:otherwise>
                         </xsl:choose> 
                        </SEDOL> 	

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

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
							<xsl:value-of select="$Position"/>
						</Quantity>

						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL12)"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>



						<Side>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name="MarketValueLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValue>
							<xsl:choose>
								<xsl:when test="number($MarketValueLocal)">
									<xsl:value-of select="$MarketValueLocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>
						
						<xsl:variable name="MarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number($MarketValueBase)">
									<xsl:value-of select="$MarketValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>
						
						<CurrencySymbol>
							<xsl:value-of select="COL2"/>
						</CurrencySymbol>
			
						
			<SMRequest>
			<xsl:value-of select="'True'"/>
			</SMRequest>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>