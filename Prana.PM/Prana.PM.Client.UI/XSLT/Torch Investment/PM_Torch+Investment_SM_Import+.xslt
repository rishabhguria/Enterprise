<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

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
	</xsl:template>

	<xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<xsl:template name="UPPER">
		<xsl:param name="text"/>
		<xsl:value-of select="translate(substring-before(normalize-space(COL7),' '), $smallcase, $uppercase)"/>
	</xsl:template>

	<xsl:template name="UPPERPutCall">
		<xsl:param name="text"/>
		<xsl:value-of select="translate(substring(normalize-space(COL3),1,1), $smallcase, $uppercase)"/>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C' or $PutOrCall='c'">
			<xsl:choose>
				<xsl:when test="$Month=01 or $Month=1">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 or $Month=2">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 or $Month=3">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 or $Month=4">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 or $Month=5">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 or $Month=6">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07 or $Month=7">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08 or $Month=8">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 or $Month=9">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P' or $PutOrCall='p'">
			<xsl:choose>
				<xsl:when test="$Month=01 or $Month=1">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 or $Month=2">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 or $Month=3">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 or $Month=4">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 or $Month=5">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 or $Month=6">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07 or $Month=7">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08 or $Month=8">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 or $Month=9">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
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
			<xsl:call-template name="UPPER">
				<xsl:with-param name="text" select="substring-before(normalize-space(COL7),' ')"/>
			</xsl:call-template>
		</xsl:variable>
	    	<xsl:variable name="ExpiryDay">
          <xsl:value-of select="substring-before(substring-after(normalize-space(COL7),'/'),'/')"/>
        </xsl:variable>
        <xsl:variable name="ExpiryMonth">
          <xsl:value-of select="substring(substring-before(normalize-space(COL7),'/'),string-length(substring-before(normalize-space(COL7),'/'))-1)"/>
        </xsl:variable>
        <xsl:variable name="ExpiryYear">
            <xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL7),'/'),'/'),' '),string-length(substring-before(substring-after(substring-after(normalize-space(COL7),'/'),'/'),' '))-1)"/>
        </xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(normalize-space(COL3),1,1)"/>
		</xsl:variable>	
		<xsl:variable name="StrikePrice">		 
			<xsl:value-of select="format-number(normalize-space(COL9),'##.00')"/>			
		</xsl:variable>
		<xsl:variable name="MonthCodeVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="($ExpiryMonth)"/>
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
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varAsset">
					<xsl:value-of select="normalize-space(COL5)"/>
				</xsl:variable>

				<xsl:variable name="varAssetType">
					<xsl:choose>
						<xsl:when test="normalize-space(COL3)='Put' or normalize-space(COL3)='put' or normalize-space(COL3)='Call' or normalize-space(COL3)='call'">
							<xsl:value-of select="'EquityOption'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Equity'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test="$varAssetType='EquityOption'">

					<PositionMaster>			

						<xsl:variable name="varOption">
							<xsl:call-template name="Option">
								<xsl:with-param name="Symbol" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>						

						<TickerSymbol>
							<xsl:value-of select="$varOption"/>
						</TickerSymbol>

						<xsl:variable name="varPutORCall">
							<xsl:call-template name="UPPERPutCall">
								<xsl:with-param name="text" select="substring(normalize-space(COL3),1,1)"/>
							</xsl:call-template>
						</xsl:variable>

						<PutOrCall>
							<xsl:choose>
								<xsl:when test="$varPutORCall='P' or $varPutORCall='p' ">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:when test="$varPutORCall='C' or $varPutORCall='c'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>

						<Multiplier>
							<xsl:value-of select="normalize-space(COL13)"/>
						</Multiplier>

						<xsl:variable name="varExpiryDate">
							<xsl:choose>
							    <xsl:when test="string-length(normalize-space(COL10))=9">
									<xsl:value-of select="concat(0,normalize-space(COL10))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL10)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<ExpirationDate>
							<xsl:value-of select="$varExpiryDate"/>
						</ExpirationDate>

						<xsl:variable name="UnderlyingSymbol">
							<xsl:call-template name="UPPER">
								<xsl:with-param name="text" select="substring-before(normalize-space(COL7),' ')"/>
							</xsl:call-template>
						</xsl:variable>

						<UnderLyingSymbol>
							<xsl:value-of select="$UnderlyingSymbol"/>
						</UnderLyingSymbol>		
						
						<xsl:variable name="StrikePrice1">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL9)"/>
							</xsl:call-template>
						</xsl:variable>

						<StrikePrice>
							<xsl:value-of select="$StrikePrice1"/>
						</StrikePrice>

						<xsl:variable name="StrikePrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL9) * 1000"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="varStrikePrice2">
							<xsl:choose>
								<xsl:when test="string-length($StrikePrice)=7">
									<xsl:value-of select="concat('0',$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice)=6">
									<xsl:value-of select="concat('00',$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice)=5">
									<xsl:value-of select="concat('000',$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice)=4">
									<xsl:value-of select="concat('0000',$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice)=3">
									<xsl:value-of select="concat('00000',$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice)=2">
									<xsl:value-of select="concat('000000',$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice)=1">
									<xsl:value-of select="concat('0000000',$StrikePrice)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$StrikePrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="ExpiryDay">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL10),'/'),'/')"/>
						</xsl:variable>											

						<xsl:variable name="ExpiryMonth">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space(COL10),'/'))=1">
									<xsl:value-of select="concat('0',substring-before(normalize-space(COL10),'/'))"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(normalize-space(COL10),'/'))=2">
									<xsl:value-of select="substring-before(normalize-space(COL10),'/')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="ExpiryYear">
							<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL10),'/'),'/'),3,2)"/>
						</xsl:variable>

						<xsl:variable name="OsiOption">
							<xsl:choose>
								<xsl:when test="string-length($UnderlyingSymbol)=1">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',' ',' ',$ExpiryYear,$ExpiryMonth,$ExpiryDay,$varPutORCall,$varStrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=2">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',' ',$ExpiryYear,$ExpiryMonth,$ExpiryDay,$varPutORCall,$varStrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=3">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',$ExpiryYear,$ExpiryMonth,$ExpiryDay,$varPutORCall,$varStrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=4">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',$ExpiryYear,$ExpiryMonth,$ExpiryDay,$varPutORCall,$varStrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=5">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',$ExpiryYear,$ExpiryMonth,$ExpiryDay,$varPutORCall,$varStrikePrice2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<OSIOptionSymbol>
							<xsl:value-of select="$OsiOption"/>						
						</OSIOptionSymbol>

						<IDCOOptionSymbol>
							<xsl:value-of select="concat($OsiOption,'U')"/>							
						</IDCOOptionSymbol>

						<AUECID>
							<xsl:value-of select="12"/>
						</AUECID>
						
						
					<xsl:variable name="varBloombergSymbol">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>
						
						<LongName>
							<xsl:value-of select="$varBloombergSymbol"/>
						</LongName>
						
						
						<BloombergSymbol>
						<xsl:value-of select="$varBloombergSymbol"/>
						</BloombergSymbol>

						<UDASector>
							<xsl:value-of select="'Undefined'"/>
						</UDASector>

						<UDASubSector>
							<xsl:value-of select="'Undefined'"/>
						</UDASubSector>

						<UDASecurityType>
							<xsl:value-of select="'Undefined'"/>
						</UDASecurityType>

						<UDAAssetClass>
							<xsl:value-of select="'Undefined'"/>
						</UDAAssetClass>

						<UDACountry>
							<xsl:value-of select="'Undefined'"/>
						</UDACountry>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
