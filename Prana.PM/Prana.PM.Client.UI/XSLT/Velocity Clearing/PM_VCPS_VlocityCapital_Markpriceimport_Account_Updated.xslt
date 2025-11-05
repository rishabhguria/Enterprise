<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
  <xsl:output method="xml" indent="yes" />
	
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
          <xsl:value-of select="substring-before(normalize-space(COL9),' ')"/>        
    </xsl:variable>
    <xsl:variable name="ExpiryDay">     
       <xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),5,2)"/>      
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),7,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL9),' '),8) div 1000,'#.00')"/>
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
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL12)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and (normalize-space(COL4)='115842' or normalize-space(COL4)='228427' or normalize-space(COL4)='378758'
                                or normalize-space(COL4)='596153' or normalize-space(COL4)='870710' or normalize-space(COL4)='460740' 
								or normalize-space(COL4)='378291' or normalize-space(COL4)='228621'  or normalize-space(COL4)='446367' or normalize-space(COL4)='830211'
								or normalize-space(COL4)='145022' or normalize-space(COL4)='265410' or normalize-space(COL4)='897001' or normalize-space(COL4)='265410'
								or normalize-space(COL4)='456003' or normalize-space(COL4)='557308' or normalize-space(COL4)='675557')">	
				
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Velocity'"/>
						</xsl:variable>						

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>
						
						<xsl:variable name="varAsset">
							<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),7,1)"/>
						</xsl:variable>
						
						<xsl:variable name="varAssetType">
                          <xsl:choose>
                            <xsl:when test="contains($varAsset,'C') or contains($varAsset,'P')">
                              <xsl:value-of select="'EquityOption'" />
                            </xsl:when>                          
							<xsl:otherwise>
								<xsl:value-of select="'Equity'"/>
							</xsl:otherwise>
                          </xsl:choose>
                        </xsl:variable>
						
						<PBSymbol>
							<xsl:value-of select ="substring(substring-after(normalize-space(COL9),' '),7)"/>
						</PBSymbol>


						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAssetType='EquityOption'">
                                  <xsl:call-template name="Option">
                                    <xsl:with-param name="Symbol" select="normalize-space(COL9)"/>
                                  </xsl:call-template>
                                </xsl:when>
								<xsl:when test="$varSymbol!='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<!-- <xsl:variable name ="PB_FUND_NAME">		  -->
							<!-- <xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/> -->
						<!-- </xsl:variable> -->
						
						<xsl:variable name="PB_FUND_NAME">
                          <xsl:choose>
                            <xsl:when test="COL6 = '' or COL6='*'">
							<xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5))"/>
                            </xsl:when>                          
							<xsl:otherwise>
							<xsl:value-of select="concat(normalize-space(COL2),'-',normalize-space(COL3),'-',normalize-space(COL4),'-',normalize-space(COL5),'-',normalize-space(COL6))"/>
							</xsl:otherwise>
                          </xsl:choose>
                        </xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<xsl:variable name ="PRANA_FUND_ID">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PBFundCode"/>
						</xsl:variable>
                        
						<AccountID>
						  <xsl:choose>
								<xsl:when test="$PRANA_FUND_ID!=''">
									<xsl:value-of select="$PRANA_FUND_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountID>
						
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

						<xsl:variable name ="varMarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*(-1)"/>
								</xsl:when>
								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="varDate">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select ="''"/>
						</Date>
													
					
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


